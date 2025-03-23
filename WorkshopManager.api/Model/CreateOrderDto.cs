namespace WorkshopManager.api.Model
{
    public class CreateOrderDto
    {
        public string? Description { get; set; }  
        public string ClientName { get; set; }  
        public string ClientPhoneNumber { get; set; }  
        public string? ClientEmail { get; set; }  
        public string? ClientAddress { get; set; }  
        public string VIN { get; set; }  
        public string Make { get; set; }  
        public string Model { get; set; } 
        public int Year { get; set; }  
        public string LicensePlate { get; set; } 
    }
}
