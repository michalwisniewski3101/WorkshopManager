using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos;
using WorkshopManager.api.Repos.Interfaces;


namespace WorkshopManager.Tests;

[TestFixture]
public class OrderRepositoryTests
{
    private WorkshopContext _context;
    private OrderRepository _orderRepository;
    private Mock<IVehicleRepository> _vehicleRepositoryMock;

    [SetUp]
    public void Setup()
    {
 
        var options = new DbContextOptionsBuilder<WorkshopContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new WorkshopContext(options);
        _context.Database.EnsureCreated();


        _vehicleRepositoryMock = new Mock<IVehicleRepository>();

        _orderRepository = new OrderRepository(_context, _vehicleRepositoryMock.Object);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllOrdersAsync_ReturnsAllOrders()
    {
         
        var order1 = new Order { Id = Guid.NewGuid(), ClientCode = "CODE1" };
        var order2 = new Order { Id = Guid.NewGuid(), ClientCode = "CODE2" };
        _context.Orders.AddRange(order1, order2);
        await _context.SaveChangesAsync();

         
        var orders = await _orderRepository.GetAllOrdersAsync();

         
        Assert.AreEqual(2, orders.Count());
    }

    [Test]
    public async Task GetOrderByIdAsync_ExistingId_ReturnsOrder()
    {
         
        var order = new Order { Id = Guid.NewGuid(), ClientCode = "CODE123" };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

         
        var result = await _orderRepository.GetOrderByIdAsync(order.Id);

         
        Assert.IsNotNull(result);
        Assert.AreEqual(order.Id, result.Id);
    }

    [Test]
    public async Task GetOrderByIdAsync_NonExistingId_ReturnsNull()
    {
         
        var result = await _orderRepository.GetOrderByIdAsync(Guid.NewGuid());

         
        Assert.IsNull(result);
    }

    [Test]
    public async Task GetOrdersByWorkerId_ReturnsOrdersForWorker()
    {
         
        var workerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

   
        _context.Orders.Add(new Order { Id = orderId, ClientCode = "ORDWORKER" });
   
        _context.ServiceSchedules.Add(new ServiceSchedule
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
  
            Mechanics = new List<Guid> { workerId }
        });
        await _context.SaveChangesAsync();

         
        var orders = await _orderRepository.GetOrdersByWorkerId(workerId);

         
        Assert.IsNotNull(orders);
        Assert.AreEqual(1, orders.Count());
        Assert.AreEqual(orderId, orders.First().Id);
    }

    [Test]
    public async Task GetOrdersByVehicleId_ReturnsOrdersForVehicle()
    {
         
        var vehicleId = Guid.NewGuid();
        var order1 = new Order { Id = Guid.NewGuid(), VehicleId = vehicleId, ClientCode = "VHC001" };
        var order2 = new Order { Id = Guid.NewGuid(), VehicleId = Guid.NewGuid(), ClientCode = "OTHER" };
        _context.Orders.AddRange(order1, order2);
        await _context.SaveChangesAsync();

         
        var orders = await _orderRepository.GetOrdersByVehicleId(vehicleId);

         
        Assert.IsNotNull(orders);
        Assert.AreEqual(1, orders.Count());
        Assert.AreEqual(order1.Id, orders.First().Id);
    }

    [Test]
    public async Task GetOrderByCodeAsync_ExistingCode_ReturnsOrder()
    {
         
        var order = new Order { Id = Guid.NewGuid(), ClientCode = "CLIENT123" };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

         
        var result = await _orderRepository.GetOrderByCodeAsync("CLIENT123");

         
        Assert.IsNotNull(result);
        Assert.AreEqual("CLIENT123", result.ClientCode);
    }

    [Test]
    public async Task AddOrderAsync_ValidOrderDto_AddsOrderAndReturnsNewOrder()
    {
         
        var vehicleId = Guid.NewGuid();

        _vehicleRepositoryMock
            .Setup(repo => repo.GetOrAddVehicleAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(vehicleId);

        var orderDto = new CreateOrderDto
        {
            VIN = "123ABC",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2020,
            LicensePlate = "ABC123",
            Description = "Test order",
            ClientName = "Jan Nowak",
            ClientPhoneNumber = "123456789",
            ClientEmail = "jan.nowak@example.com",
            ClientAddress = "Ulica Testowa 1",

        };

         
        var newOrder = await _orderRepository.AddOrderAsync(orderDto);

         
        Assert.IsNotNull(newOrder);
        Assert.AreEqual(vehicleId, newOrder.VehicleId);
        Assert.AreEqual(OrderStatus.Pending, newOrder.OrderStatus);
        Assert.AreEqual(orderDto.Description, newOrder.Description);
        Assert.IsFalse(string.IsNullOrEmpty(newOrder.ClientCode));
    }

    [Test]
    public async Task UpdateOrderAsync_ExistingOrder_UpdatesOrder()
    {
         
        var order = new Order { Id = Guid.NewGuid(), ClientCode = "UPD001", Description = "Old description" };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

 
        order.Description = "New description";

         
        var result = await _orderRepository.UpdateOrderAsync(order);

         
        Assert.IsTrue(result);
        var updatedOrder = await _context.Orders.FindAsync(order.Id);
        Assert.AreEqual("New description", updatedOrder.Description);
    }

    [Test]
    public async Task DeleteOrderAsync_ExistingOrder_DeletesOrder()
    {
         
        var order = new Order { Id = Guid.NewGuid(), ClientCode = "DEL001" };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

         
        var result = await _orderRepository.DeleteOrderAsync(order.Id);

         
        Assert.IsTrue(result);
        var deletedOrder = await _context.Orders.FindAsync(order.Id);
        Assert.IsNull(deletedOrder);
    }

    [Test]
    public async Task DeleteOrderAsync_NonExistingOrder_ReturnsFalse()
    {
         
        var result = await _orderRepository.DeleteOrderAsync(Guid.NewGuid());

         
        Assert.IsFalse(result);
    }

    [Test]
    public void OrderExists_ReturnsTrueIfOrderExists()
    {
         
        var order = new Order { Id = Guid.NewGuid(), ClientCode = "EXIST001" };
        _context.Orders.Add(order);
        _context.SaveChanges();

         
        var exists = _orderRepository.OrderExists(order.Id);

         
        Assert.IsTrue(exists);
    }

    [Test]
    public void OrderExists_ReturnsFalseIfOrderDoesNotExist()
    {
         
        var exists = _orderRepository.OrderExists(Guid.NewGuid());

         
        Assert.IsFalse(exists);
    }
}
