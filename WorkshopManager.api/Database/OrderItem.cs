public class OrderItem : IEntity
{
    public Guid Id { get; set; }
    public Guid InventoryItemId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }


}
