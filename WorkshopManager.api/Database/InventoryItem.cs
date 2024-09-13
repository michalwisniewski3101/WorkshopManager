public class InventoryItem : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ProductNumber { get; set; }
    public int QuantityInStock { get; set; }
    public decimal UnitPrice { get; set; }
    public int ReorderLevel { get; set; }
    public string Supplier { get; set; }


}
