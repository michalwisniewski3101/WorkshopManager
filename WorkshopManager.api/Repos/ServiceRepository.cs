using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopManager.api.Database;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;
using static ServiceSchedule;

namespace WorkshopManager.api.Repos
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly WorkshopContext _context;

        public ServiceRepository(WorkshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Service>> GetServicesAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service?> GetServiceByIdAsync(Guid id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceSchedule>> GetServiceSchedulesByOrderIdAsync(Guid orderId)
        {
            return await _context.ServiceSchedules
                .Where(ss => ss.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<ServiceSchedule?> GetServiceScheduleByIdAsync(Guid id)
        {
            return await _context.ServiceSchedules.FindAsync(id);
        }
        public async Task<ServiceSchedule?> DeleteServiceSchedule(Guid id)
        {
            var serviceSchedule = await _context.ServiceSchedules.FindAsync(id);
            
            if (serviceSchedule != null)
            {
                var serviceScheduleOrderId = serviceSchedule.OrderId;
                _context.ServiceSchedules.Remove(serviceSchedule);
                await _context.SaveChangesAsync();
                if (serviceSchedule.OrderId != null)
                {
                    await UpdateOrderStatus(serviceSchedule.OrderId);
                }


                if (serviceSchedule.ServiceStatus != ServiceStatus.WaitingForParts)
                {
                    foreach (var orderItem in serviceSchedule.OrderItems)
                    {

                        var inventoryItem = await _context.InventoryItems
                            .FirstOrDefaultAsync(item => item.Id == orderItem.InventoryItemId);

                        if (inventoryItem == null)
                        {
                            throw new Exception($"Nie znaleziono przedmiotu o ID {orderItem.InventoryItemId} w magazynie.");
                        }

                        inventoryItem.QuantityInStock += orderItem.Quantity;


                    }

                    await _context.SaveChangesAsync();
                }





                return serviceSchedule;
            }
           

            return null;
        }

        public async Task<Guid> AddServiceAsync(Service service)
        {
            service.Id = Guid.NewGuid();
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service.Id;
        }

        public async Task<Guid> AddServiceScheduleAsync(ServiceSchedule serviceSchedule)
        {
            serviceSchedule.Id = Guid.NewGuid();

            serviceSchedule.ServiceStatus = ServiceStatus.Pending;
            // Iteracja po wszystkich pozycjach zamówienia
            foreach (var orderItem in serviceSchedule.OrderItems)
            {
                // Pobieranie ceny jednostkowej z magazynu
                var inventoryItem = await _context.InventoryItems
                    .FirstOrDefaultAsync(item => item.Id == orderItem.InventoryItemId);

                if (inventoryItem == null)
                {
                    throw new Exception($"Nie znaleziono przedmiotu o ID {orderItem.InventoryItemId} w magazynie.");
                }
                bool isAvailable = await IsOrderItemAvailable(orderItem.InventoryItemId, orderItem.Quantity);
                if (!isAvailable)
                {
                    serviceSchedule.ServiceStatus = ServiceStatus.WaitingForParts;
                }
                else
                {
                    serviceSchedule.ServiceStatus = ServiceStatus.Pending;
                    inventoryItem.QuantityInStock -= orderItem.Quantity;
                }


                orderItem.TotalPrice = inventoryItem.UnitPrice * orderItem.Quantity;
            }

            _context.ServiceSchedules.Add(serviceSchedule);
            await _context.SaveChangesAsync();
            if (serviceSchedule.OrderId != null)
            {
                await UpdateOrderStatus(serviceSchedule.OrderId);
            }
            return serviceSchedule.Id;
        }

        public async Task UpdateServiceScheduleStatusAsync(Guid id, ServiceStatus newStatus)
        {
            var serviceSchedule = await _context.ServiceSchedules.FindAsync(id);
            bool canChange = true;

            if (serviceSchedule != null)
            {

                if (serviceSchedule.ServiceStatus == ServiceStatus.WaitingForParts)
                {


                    foreach (var orderItem in serviceSchedule.OrderItems)
                    {

                        var inventoryItem = await _context.InventoryItems
                                .FirstOrDefaultAsync(item => item.Id == orderItem.InventoryItemId);

                        if (orderItem.Quantity > inventoryItem.QuantityInStock)
                        {
                            canChange = false;
                            continue;
                        }




                    }
                }

                if (canChange)
                {
                    serviceSchedule.ServiceStatus = newStatus;
                }
                
                await _context.SaveChangesAsync();
                if (serviceSchedule.OrderId!=null)
                {
                    await UpdateOrderStatus(serviceSchedule.OrderId);
                }
            }
        }
        


        public async Task<bool> ServiceExistsAsync(Guid id)
        {
            return await _context.Services.AnyAsync(e => e.Id == id);
        }
        public async Task<bool> IsOrderItemAvailable(Guid id, int quantity)
        {
            var item = await _context.InventoryItems.FindAsync(id);

            return item != null && item.QuantityInStock >= quantity;
        }
        public async Task UpdateOrderStatus(Guid orderId)
        {
            


                var serviceStatuses = await _context.ServiceSchedules
                    .Where(ss => ss.OrderId == orderId)
                    .Select(ss => ss.ServiceStatus)
                    .ToListAsync();


                OrderStatus newOrderStatus;

                if (serviceStatuses.All(status => status == ServiceStatus.Pending))
                {
                    newOrderStatus = OrderStatus.Pending;
                }
                else if (serviceStatuses.All(status => status == ServiceStatus.Completed))
                {
                    newOrderStatus = OrderStatus.WaitingForApproval;
                }
                else if (serviceStatuses.All(status => status == ServiceStatus.Canceled))
                {
                    newOrderStatus = OrderStatus.OnHold;
                }



                else if (serviceStatuses.All(status => status == ServiceStatus.Pending || status == ServiceStatus.Canceled))
                {
                    newOrderStatus = OrderStatus.Pending;
                }
                else if (serviceStatuses.All(status => status == ServiceStatus.Completed || status == ServiceStatus.Canceled))
                {
                    newOrderStatus = OrderStatus.WaitingForApproval;
                }


                else if (serviceStatuses.Any(status => status == ServiceStatus.InProgress))
                {
                    newOrderStatus = OrderStatus.InProgress;
                }
                else if (serviceStatuses.Any(status => status == ServiceStatus.WaitingForParts))
                {
                    newOrderStatus = OrderStatus.OnHold;
                }

                else
                {
                    // Opcjonalny przypadek dla innych kombinacji statusów
                    return;
                }

                // Aktualizacja statusu zamówienia
                var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.OrderStatus = newOrderStatus;

                // Sumowanie total price dla wszystkich OrderItems powiązanych z danym OrderId
                var totalCost = await _context.ServiceSchedules
                    .Where(ss => ss.OrderId == orderId)
                    .SelectMany(ss => ss.OrderItems)
                    .SumAsync(item => item.TotalPrice);

                order.TotalCost = totalCost;

                await _context.SaveChangesAsync();
            }


        }
    } 
}

