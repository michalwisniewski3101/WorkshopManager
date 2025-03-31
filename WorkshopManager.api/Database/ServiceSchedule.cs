using WorkshopManager.api.Model;

public class ServiceSchedule
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime? ServiceDateStart { get; set; }
    public DateTime? ServiceDateEnd { get; set; }
    public Guid? ServiceId { get; set; }
    public List<Guid>? Mechanics { get; set; } = new List<Guid>();
    public ServiceStatus? ServiceStatus { get; set; }
    public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();



    public class OrderItem
    {
        public Guid InventoryItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
