public class Vehicle : IEntity
{
    public Guid Id { get; set; }  

    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string VIN { get; set; }


}
