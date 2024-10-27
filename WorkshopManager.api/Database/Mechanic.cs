using WorkshopManager.api.Database;

public class Mechanic : IEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid SpecialtyId { get; set; }
    public int ExperienceLevel { get; set; }
    public DateTime DateJoined { get; set; }


}
