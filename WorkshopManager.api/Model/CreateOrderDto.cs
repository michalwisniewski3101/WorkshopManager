namespace WorkshopManager.api.Model
{
    public class CreateOrderDto
    {
        public List<ServiceSchedule> ServiceSchedules { get; set; } = new List<ServiceSchedule>();
        public string? Description { get; set; }  // Opis zamówienia
        public string ClientName { get; set; }  // Imię klienta
        public string ClientPhoneNumber { get; set; }  // Numer telefonu klienta
        public string? ClientEmail { get; set; }  // E-mail klienta
        public string? ClientAddress { get; set; }  // Adres klienta
        public string? ClientCode { get; set; }  // Kod klienta
        public string VIN { get; set; }  // Numer identyfikacyjny pojazdu
        public string Make { get; set; }  // Marka pojazdu
        public string Model { get; set; }  // Model pojazdu
        public int Year { get; set; }  // Rok produkcji
        public string LicensePlate { get; set; }  // Tablica rejestracyjna
    }
}
