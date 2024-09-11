public class Service : IEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string ServiceDescription { get; set; }
    public decimal ServiceCost { get; set; }
    public TimeSpan ServiceDuration { get; set; }

    // Navigation properties
    public Order Order { get; set; }
}
