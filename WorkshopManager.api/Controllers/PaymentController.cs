using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentRequest request)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card", "blik" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "pln",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Usługa serwisowa"
                        },
                        UnitAmount = 50000 // kwota w groszach (500 zł)
                    },
                    Quantity = 1
                }

            },
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/success",
                CancelUrl = "http://localhost:3000/cancel",
                InvoiceCreation = new SessionInvoiceCreationOptions
                {
                    Enabled = request.NeedsInvoice
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Ok(new { url = session.Url });
        }





    }

}
