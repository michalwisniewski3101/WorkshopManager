using WorkshopManager.api.Model;

namespace WorkshopManager.api.Repos.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<Order> AddOrderAsync(CreateOrderDto orderDto);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(Guid id);
        bool OrderExists(Guid id);
    }
}
