public class Order : IEntity
{
    public Guid Id { get; set; }  // GUID jako identyfikator

    public Guid VehicleId { get; set; }  // Relacja z Vehicle (też GUID)
    public DateTime OrderDate { get; set; }
    public DateTime? EstimatedCompletionDate { get; set; }
    public decimal TotalCost { get; set; }
    public string OrderStatus { get; set; }
    public string Description { get; set; }

    // Nawigacja
    public Vehicle Vehicle { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public ICollection<Service> Services { get; set; }
}
