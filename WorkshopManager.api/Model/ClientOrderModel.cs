namespace WorkshopManager.api.Model
{
    public class ClientOrderModel
    {
        public List<ServiceSchedule> Services { get; set; } = new List<ServiceSchedule>();
        public DateTime OrderDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public decimal TotalCost { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
        public string ClientName { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public string ClientCode { get; set; }

    }
}
