namespace WorkshopManager.api.Database
{
    public class MechanicSpecialty : IEntity
    {
        public Guid Id { get; set; }
        public string SpecialtyName { get; set; }
    }
}
