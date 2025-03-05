namespace WorkshopManager.api.Model
{
    public class PaymentRequest
    {
        public bool NeedsInvoice { get; set; }
        public Guid OrderId { get; set; }
    }
}
