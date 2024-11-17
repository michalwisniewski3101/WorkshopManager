public class Service : IEntity
{
    public Guid Id { get; set; }
    public string ServiceDescription { get; set; }
    public decimal? ServiceCost { get; set; }
    public int? ServiceDuration { get; set; }

}
