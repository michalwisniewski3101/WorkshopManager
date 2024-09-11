public class OrderItem : IEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid InventoryItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // Navigation properties
    public Order Order { get; set; }
    public InventoryItem InventoryItem { get; set; }
}
