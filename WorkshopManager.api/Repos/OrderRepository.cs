using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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

  
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

    
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<Order>> GetOrdersByWorkerId(Guid workerId)
        {

            var serviceSchedules = await _context.ServiceSchedules.ToListAsync();

            var orderIds = serviceSchedules
            .Where(ss => ss.Mechanics != null && ss.Mechanics.Contains(workerId))
            .Select(ss => ss.OrderId)
            .Distinct()
            .ToList();



            return await _context.Orders
        .Where(o => orderIds.Contains(o.Id))
        .ToListAsync();

        }
        public async Task<IEnumerable<Order>> GetOrdersByVehicleId(Guid VehicleId)
        {

            

            return await _context.Orders
        .Where(ss => ss.VehicleId == VehicleId)
                .ToListAsync();
        }
        public async Task<Order> GetOrderByCodeAsync(string code)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.ClientCode == code);
        }
        public async Task<Order> AddOrderAsync(CreateOrderDto orderDto)
        {
  
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
                ClientCode = GenerateClientCode()
            };



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


        public bool OrderExists(Guid id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
        public static string GenerateClientCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new char[6];

            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[6];
                rng.GetBytes(bytes);

                for (int i = 0; i < code.Length; i++)
                {
                    code[i] = chars[bytes[i] % chars.Length];
                }
            }

            return new string(code);
        }
    }
}
