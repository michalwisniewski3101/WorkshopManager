using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {

        private readonly WorkshopContext _context;

        public PaymentController(WorkshopContext context)
        {
            _context = context;
        }


        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentRequest request)
        {

            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return NotFound("Nie znaleziono zamówienia");
            }

 
            var serviceSchedules = await _context.ServiceSchedules
                .Where(ss => ss.OrderId == request.OrderId)
                .ToListAsync();


            var taxRateService = new TaxRateService();
            var options1 = new TaxRateListOptions
            {
                Limit = 10
            };



        
            StripeList<TaxRate> taxRates = taxRateService.List(options1);
    
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var schedule in serviceSchedules)
            {
                if (schedule.OrderItems != null)
                {
                    foreach (var item in schedule.OrderItems)
                    {
                        var inventoryItem = await _context.InventoryItems.FindAsync(item.InventoryItemId);
                        string taxRateId = null;
                        if (inventoryItem?.TaxRate != null)
                        {
                            var matchingTaxRate = taxRates.Data.FirstOrDefault(tr => tr.Percentage == inventoryItem?.TaxRate);
                            taxRateId = matchingTaxRate?.Id;
                        }

                        var productName = inventoryItem?.Name ?? "Usługa serwisowa";
                        lineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "pln",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = productName
                                },
                             
                                UnitAmount = Convert.ToInt64((item.TotalPrice/item.Quantity) * 100)
                            },
                            Quantity = item.Quantity,
                            TaxRates = taxRateId != null ? new List<string> { taxRateId } : null
                        });
                    }
                }
            }

            if (!lineItems.Any())
            {
                return BadRequest("Brak pozycji zamówienia do opłacenia.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card", "blik" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/client/",
                CancelUrl = "http://localhost:3000/client/",
                InvoiceCreation = new SessionInvoiceCreationOptions
                {
                    Enabled = request.NeedsInvoice
                }
            };

            var sessionService = new SessionService();
            Session session = await sessionService.CreateAsync(options);

            return Ok(new { url = session.Url });
        }





    }

}
