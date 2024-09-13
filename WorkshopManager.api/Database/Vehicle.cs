public class Vehicle : IEntity
{
    public Guid Id { get; set; }  // GUID jako identyfikator

    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string LicensePlate { get; set; }
    public string VIN { get; set; }

    // Dane właściciela
    public string OwnerName { get; set; }
    public string OwnerPhoneNumber { get; set; }
    public string OwnerEmail { get; set; }
    public string OwnerAddress { get; set; }

}
