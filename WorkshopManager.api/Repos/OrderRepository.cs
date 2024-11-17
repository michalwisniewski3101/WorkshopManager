using Microsoft.EntityFrameworkCore;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.api.Repos
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WorkshopContext _context;
        private readonly IVehicleRepository _vehicleRepository;

        public OrderRepository(WorkshopContext context, IVehicleRepository vehicleRepository)
        {
            _context = context;
            _vehicleRepository = vehicleRepository;
        }

        // Pobierz wszystkie zamówienia
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.Services).ToListAsync();
        }

        // Pobierz zamówienie po ID
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        // Dodaj nowe zamówienie
        public async Task<Order> AddOrderAsync(CreateOrderDto orderDto)
        {
            // Uzyskujemy lub tworzymy pojazd na podstawie danych VIN (jeśli pojazd nie istnieje)
            var vehicleId = await _vehicleRepository.GetOrAddVehicleAsync(orderDto.VIN, orderDto.Make, orderDto.Model, orderDto.Year, orderDto.LicensePlate);

            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicleId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                Description = orderDto.Description,
                ClientName = orderDto.ClientName,
                ClientPhoneNumber = orderDto.ClientPhoneNumber,
                ClientEmail = orderDto.ClientEmail,
                ClientAddress = orderDto.ClientAddress,
                ClientCode = orderDto.ClientCode
            };


            if (orderDto.ServiceSchedules != null)
            {
                foreach (var schedule in orderDto.ServiceSchedules)
                {
                    newOrder.Services.Add(new ServiceSchedule
                    {
                        Id = schedule.Id,
                        ServiceDateStart = schedule.ServiceDateStart,
                        ServiceDateEnd = schedule.ServiceDateEnd,
                        Service = schedule.Service,
                        Mechanics = schedule.Mechanics,
                        ServiceStatus = schedule.ServiceStatus,
                        OrderItems = schedule.OrderItems
                    });

                }

            }
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return newOrder;
        }

        
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // Usuń zamówienie
        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // Sprawdź, czy zamówienie istnieje
        public bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
