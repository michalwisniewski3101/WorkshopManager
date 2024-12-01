using WorkshopManager.api.Model;

public class Order : IEntity
{
    public Guid Id { get; set; }  
    public Guid VehicleId { get; set; }  
    public DateTime OrderDate { get; set; }
    public DateTime? EstimatedCompletionDate { get; set; }
    public decimal? TotalCost { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public string? Description { get; set; }
    public string? ClientName { get; set; }
    public string? ClientPhoneNumber { get; set; }
    public string? ClientEmail { get; set; }
    public string? ClientAddress { get; set; }
    public string? ClientCode { get; set; }

}
