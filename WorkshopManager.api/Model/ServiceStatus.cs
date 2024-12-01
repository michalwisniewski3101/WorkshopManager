namespace WorkshopManager.api.Model
{
    public enum ServiceStatus
    {
        Pending,              // Zamówienie oczekujące na realizację
        WaitingForParts,      // Czekanie na części
        InProgress,           // Zamówienie w trakcie realizacji
        Completed,            // Zamówienie zakończone
        Canceled,             // Zamówienie anulowane
    }

}




