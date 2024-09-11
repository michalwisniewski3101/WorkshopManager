public class Invoice : IEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; }

    // Navigation properties
    public Order Order { get; set; }
}
