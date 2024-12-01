using WorkshopManager.api.Model;

namespace WorkshopManager.api.Repos.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetServicesAsync();
        Task<Service?> GetServiceByIdAsync(Guid id);
        Task<IEnumerable<ServiceSchedule>> GetServiceSchedulesByOrderIdAsync(Guid orderId);
        Task<ServiceSchedule?> GetServiceScheduleByIdAsync(Guid id);
        Task<Guid> AddServiceAsync(Service service);
        Task<Guid> AddServiceScheduleAsync(ServiceSchedule serviceSchedule);
        Task UpdateServiceScheduleStatusAsync(Guid id, ServiceStatus newStatus);
        Task<bool> ServiceExistsAsync(Guid id);
    }
}
