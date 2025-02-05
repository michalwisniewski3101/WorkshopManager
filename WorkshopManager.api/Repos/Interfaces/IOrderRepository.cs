using WorkshopManager.api.Model;

namespace WorkshopManager.api.Repos.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> GetOrderByCodeAsync(string code);
        Task<Order> AddOrderAsync(CreateOrderDto orderDto);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByWorkerId(Guid workerId);
        Task<IEnumerable<Order>> GetOrdersByVehicleId(Guid VehicleId);
        bool OrderExists(Guid id);
        
    }
}
