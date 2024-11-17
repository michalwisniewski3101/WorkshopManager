public class ServiceSchedule
{
    public Guid Id { get; set; }
    public DateTime? ServiceDateStart { get; set; }
    public DateTime? ServiceDateEnd { get; set; }
    public Service? Service { get; set; }
    public List<Mechanic>? Mechanics { get; set; } = new List<Mechanic>();
    public string? ServiceStatus { get; set; }
    public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();


}
