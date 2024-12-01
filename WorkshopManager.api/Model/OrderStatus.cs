namespace WorkshopManager.api.Model
{
    public enum OrderStatus
    {
        Pending,              // Zamówienie oczekujące na realizację
        InProgress,           // Zamówienie w trakcie realizacji
        WaitingForApproval,   // Czekanie na zatwierdzenie
        Completed,            // Zamówienie zakończone
        OnHold,               // Zamówienie wstrzymane
        Canceled,             // Zamówienie anulowane
    }

}
