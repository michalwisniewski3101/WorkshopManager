namespace WorkshopManager.api.Model
{
    public enum OrderStatus
    {
        Pending,              // Zamówienie oczekujące na realizację
        InProgress,           // Zamówienie w trakcie realizacji
        Completed,            // Zamówienie zakończone
        Canceled,             // Zamówienie anulowane
        OnHold,               // Zamówienie wstrzymane
        WaitingForParts,      // Czekanie na części
        WaitingForApproval,   // Czekanie na zatwierdzenie
    }

}
