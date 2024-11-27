public class Order : IEntity
{
    public Guid Id { get; set; }  // GUID jako identyfikator
    public Vehicle Vehicle { get; set; }  
    public List<ServiceSchedule> Services { get; set; } = new List<ServiceSchedule>();
    public DateTime OrderDate { get; set; }
    public DateTime? EstimatedCompletionDate { get; set; }
    public decimal TotalCost { get; set; }
    public string OrderStatus { get; set; }
    public string Description { get; set; }

 
}
