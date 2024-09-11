public class ServiceSchedule
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime ServiceDate { get; set; }
    public Guid MechanicId { get; set; }
    public string ServiceStatus { get; set; }

    // Navigation properties
    public Order Order { get; set; }
    public Mechanic Mechanic { get; set; }
}
