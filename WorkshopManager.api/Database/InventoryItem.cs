public class InventoryItem : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QuantityInStock { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReorderLevel { get; set; }

    // Optional: Supplier Information
    public string Supplier { get; set; }

    // Navigation properties
    public ICollection<OrderItem> OrderItems { get; set; }
}
