using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkshopManager.api.Controllers;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private OrderController _controller;

        [SetUp]
        public void Setup()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _controller = new OrderController(_orderRepositoryMock.Object);
        }

        [Test]
        public async Task GetOrders_ReturnsListOfOrders()
        {
             
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), ClientCode = "ORD001" },
                new Order { Id = Guid.NewGuid(), ClientCode = "ORD002" }
            };

            _orderRepositoryMock.Setup(repo => repo.GetAllOrdersAsync()).ReturnsAsync(orders);

             
            var result = await _controller.GetOrders();

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(2, (okResult.Value as List<Order>).Count);
        }

        [Test]
        public async Task GetOrder_ExistingId_ReturnsOrder()
        {
             
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, ClientCode = "ORD001" };

            _orderRepositoryMock.Setup(repo => repo.GetOrderByIdAsync(orderId)).ReturnsAsync(order);

             
            var result = await _controller.GetOrder(orderId);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(orderId, ((Order)okResult.Value).Id);
        }

        [Test]
        public async Task GetOrderByCode_ExistingCode_ReturnsOrder()
        {
             
            var order = new Order { Id = Guid.NewGuid(), ClientCode = "ORD001" };

            _orderRepositoryMock.Setup(repo => repo.GetOrderByCodeAsync("ORD001")).ReturnsAsync(order);

             
            var result = await _controller.GetOrderByCode("ORD001");

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("ORD001", ((Order)okResult.Value).ClientCode);
        }

        [Test]
        public async Task GetOrderByWorker_ExistingWorkerId_ReturnsOrders()
        {
             
            var workerId = Guid.NewGuid();
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), ClientCode = "ORD001" },
                new Order { Id = Guid.NewGuid(),   ClientCode = "ORD002" }
            };

            _orderRepositoryMock.Setup(repo => repo.GetOrdersByWorkerId(workerId)).ReturnsAsync(orders);

             
            var result = await _controller.GetOrdersByWorker(workerId);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(2, (okResult.Value as List<Order>).Count);
        }


        [Test]
        public async Task PutOrder_ValidOrder_ReturnsNoContent()
        {
             
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, ClientCode = "ORD004" };

            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(order)).ReturnsAsync(true);

             
            var result = await _controller.PutOrder(orderId, order);

             
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteOrder_ExistingId_ReturnsNoContent()
        {
             
            var orderId = Guid.NewGuid();

            _orderRepositoryMock.Setup(repo => repo.DeleteOrderAsync(orderId)).ReturnsAsync(true);

             
            var result = await _controller.DeleteOrder(orderId);

             
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
