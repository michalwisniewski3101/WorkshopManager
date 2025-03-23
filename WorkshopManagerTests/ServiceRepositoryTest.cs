using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopManager.api.Database;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos;
using WorkshopManager.api.Repos.Interfaces;
using static ServiceSchedule;

namespace WorkshopManager.Tests;

[TestFixture]
public class ServiceRepositoryTests
{
    private WorkshopContext _context;
    private ServiceRepository _serviceRepository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<WorkshopContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new WorkshopContext(options);
        _context.Database.EnsureCreated();
        _serviceRepository = new ServiceRepository(_context);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    #region GetServices & GetServiceById

    [Test]
    public async Task GetServicesAsync_ReturnsAllServices()
    {
      
        var service1 = new Service { Id = Guid.NewGuid(), ServiceDescription = "Naprawa silnika", ServiceCost = 1500m, ServiceDuration = 120 };
        var service2 = new Service { Id = Guid.NewGuid(), ServiceDescription = "Wymiana oleju", ServiceCost = 200m, ServiceDuration = 30 };
        _context.Services.AddRange(service1, service2);
        await _context.SaveChangesAsync();

      
        var services = await _serviceRepository.GetServicesAsync();

    
        Assert.AreEqual(2, services.Count());
    }

    [Test]
    public async Task GetServiceByIdAsync_ExistingId_ReturnsService()
    {
    
        var service = new Service { Id = Guid.NewGuid(), ServiceDescription = "Test Service", ServiceCost = 500m, ServiceDuration = 60 };
        _context.Services.Add(service);
        await _context.SaveChangesAsync();

        var result = await _serviceRepository.GetServiceByIdAsync(service.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(service.Id, result.Id);
    }

    [Test]
    public async Task GetServiceByIdAsync_NonExistingId_ReturnsNull()
    {

        var result = await _serviceRepository.GetServiceByIdAsync(Guid.NewGuid());

        Assert.IsNull(result);
    }

    #endregion

    #region ServiceSchedules

    [Test]
    public async Task GetServiceSchedulesByOrderIdAsync_ReturnsCorrectSchedules()
    {

        var orderId = Guid.NewGuid();
        var schedule1 = new ServiceSchedule { Id = Guid.NewGuid(), OrderId = orderId };
        var schedule2 = new ServiceSchedule { Id = Guid.NewGuid(), OrderId = orderId };
        _context.ServiceSchedules.AddRange(schedule1, schedule2);
        await _context.SaveChangesAsync();

        var schedules = await _serviceRepository.GetServiceSchedulesByOrderIdAsync(orderId);

        Assert.AreEqual(2, schedules.Count());
    }

    [Test]
    public async Task GetServiceScheduleByIdAsync_ExistingId_ReturnsSchedule()
    {

        var schedule = new ServiceSchedule { Id = Guid.NewGuid() };
        _context.ServiceSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        var result = await _serviceRepository.GetServiceScheduleByIdAsync(schedule.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(schedule.Id, result.Id);
    }

    [Test]
    public async Task GetServiceScheduleByIdAsync_NonExistingId_ReturnsNull()
    {

        var result = await _serviceRepository.GetServiceScheduleByIdAsync(Guid.NewGuid());
        Assert.IsNull(result);
    }

    #endregion

    #region AddServiceAsync

    [Test]
    public async Task AddServiceAsync_AddsServiceAndReturnsId()
    {

        var service = new Service { ServiceDescription = "Nowa us³uga", ServiceCost = 300m, ServiceDuration = 45 };


        var id = await _serviceRepository.AddServiceAsync(service);


        Assert.AreNotEqual(Guid.Empty, id);
        var added = await _context.Services.FindAsync(id);
        Assert.IsNotNull(added);
        Assert.AreEqual("Nowa us³uga", added.ServiceDescription);
    }

    #endregion

    #region AddServiceScheduleAsync


    

    [Test]
    public async Task UpdateServiceScheduleStatusAsync_WithSufficientInventory_UpdatesStatus()
    {

        var inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = "Czêœæ C",
            Description = "Opis",
            ProductNumber = "C123",
            Supplier = "Dostawca",
            UnitPrice = 80m,
            QuantityInStock = 10,
            TaxRate = 23
        };
        _context.InventoryItems.Add(inventoryItem);

  
        var orderItem = new OrderItem
        {
            InventoryItemId = inventoryItem.Id,
            Quantity = 4,
            TotalPrice = 320m
        };

        var schedule = new ServiceSchedule
        {
            Id = Guid.NewGuid(),
            ServiceStatus = ServiceStatus.WaitingForParts,
            OrderItems = new List<OrderItem> { orderItem },
            OrderId = Guid.NewGuid()
        };
        _context.ServiceSchedules.Add(schedule);
        await _context.SaveChangesAsync();

  
        inventoryItem.QuantityInStock = 10; 
        await _context.SaveChangesAsync();

      
        await _serviceRepository.UpdateServiceScheduleStatusAsync(schedule.Id, ServiceStatus.Completed);

       
        var updatedSchedule = await _context.ServiceSchedules.FindAsync(schedule.Id);
   
        Assert.AreEqual(ServiceStatus.Completed, updatedSchedule.ServiceStatus);
    }

    [Test]
    public async Task UpdateServiceScheduleStatusAsync_WithInsufficientInventory_DoesNotUpdateStatus()
    {
   
        var inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = "Czêœæ D",
            Description = "Opis",
            ProductNumber = "D123",
            Supplier = "Dostawca",
            UnitPrice = 60m,
            QuantityInStock = 1, 
            TaxRate = 23
        };
        _context.InventoryItems.Add(inventoryItem);

        var orderItem = new OrderItem
        {
            InventoryItemId = inventoryItem.Id,
            Quantity = 3, 
            TotalPrice = 180m
        };

        var schedule = new ServiceSchedule
        {
            Id = Guid.NewGuid(),
            ServiceStatus = ServiceStatus.WaitingForParts,
            OrderItems = new List<OrderItem> { orderItem },
            OrderId = Guid.NewGuid()
        };
        _context.ServiceSchedules.Add(schedule);
        await _context.SaveChangesAsync();

    
        await _serviceRepository.UpdateServiceScheduleStatusAsync(schedule.Id, ServiceStatus.Completed);

      
        var updatedSchedule = await _context.ServiceSchedules.FindAsync(schedule.Id);

        Assert.AreEqual(ServiceStatus.WaitingForParts, updatedSchedule.ServiceStatus);
    }

    #endregion

    #region ServiceExists & IsOrderItemAvailable

    [Test]
    public async Task ServiceExistsAsync_ReturnsTrueIfServiceExists()
    {
   
        var service = new Service { Id = Guid.NewGuid(), ServiceDescription = "Istniej¹ca us³uga", ServiceCost = 400m, ServiceDuration = 50 };
        _context.Services.Add(service);
        await _context.SaveChangesAsync();

      
        var exists = await _serviceRepository.ServiceExistsAsync(service.Id);

  
        Assert.IsTrue(exists);
    }

    [Test]
    public async Task ServiceExistsAsync_ReturnsFalseIfServiceDoesNotExist()
    {

        var exists = await _serviceRepository.ServiceExistsAsync(Guid.NewGuid());
 
        Assert.IsFalse(exists);
    }

    [Test]
    public async Task IsOrderItemAvailable_ReturnsTrueIfSufficientInventory()
    {
     
        var inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = "Czêœæ E",
            Description = "Opis",
            ProductNumber = "E123",
            Supplier = "Dostawca",
            UnitPrice = 70m,
            QuantityInStock = 5,
            TaxRate = 23
        };
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

    
        var available = await _serviceRepository.IsOrderItemAvailable(inventoryItem.Id, 3);

    
        Assert.IsTrue(available);
    }

    [Test]
    public async Task IsOrderItemAvailable_ReturnsFalseIfInsufficientInventory()
    {
   
        var inventoryItem = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = "Czêœæ F",
            Description = "Opis",
            ProductNumber = "F123",
            Supplier = "Dostawca",
            UnitPrice = 90m,
            QuantityInStock = 2,
            TaxRate = 23
        };
        _context.InventoryItems.Add(inventoryItem);
        await _context.SaveChangesAsync();

    
        var available = await _serviceRepository.IsOrderItemAvailable(inventoryItem.Id, 3);

      
        Assert.IsFalse(available);
    }

    #endregion

    #region UpdateOrderStatus

    [Test]
    public async Task UpdateOrderStatus_UpdatesOrderStatusAndTotalCost()
    {
        
        var orderId = Guid.NewGuid();
        var order = new Order { Id = orderId, ClientCode = "ORD3" };
        _context.Orders.Add(order);

      
        var orderItem1 = new OrderItem { InventoryItemId = Guid.NewGuid(), Quantity = 2, TotalPrice = 200m };
        var orderItem2 = new OrderItem { InventoryItemId = Guid.NewGuid(), Quantity = 1, TotalPrice = 150m };

        var schedule1 = new ServiceSchedule
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ServiceStatus = ServiceStatus.Pending,
            OrderItems = new List<OrderItem> { orderItem1 }
        };
        var schedule2 = new ServiceSchedule
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            ServiceStatus = ServiceStatus.Pending,
            OrderItems = new List<OrderItem> { orderItem2 }
        };

        _context.ServiceSchedules.AddRange(schedule1, schedule2);
        await _context.SaveChangesAsync();

     
        await _serviceRepository.UpdateOrderStatus(orderId);

     
        var updatedOrder = await _context.Orders.FindAsync(orderId);
       
        Assert.AreEqual(OrderStatus.Pending, updatedOrder.OrderStatus);
   
        Assert.AreEqual(350m, updatedOrder.TotalCost);
    }

    #endregion
}
