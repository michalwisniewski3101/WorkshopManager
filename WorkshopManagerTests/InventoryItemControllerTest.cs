using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos;
using WorkshopManager.api.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

using static ServiceSchedule;

namespace WorkshopManager.Tests
{
    [TestFixture]
    public class InventoryItemControllerTests
    {
        private WorkshopContext _context;
        private InventoryItemController _controller;
        private Mock<IServiceRepository> _serviceRepositoryMock;

        [SetUp]
        public void Setup()
        {
    
            var options = new DbContextOptionsBuilder<WorkshopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new WorkshopContext(options);

 
            _serviceRepositoryMock = new Mock<IServiceRepository>();

     
            _controller = new InventoryItemController(_context, _serviceRepositoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetInventoryItems_ReturnsAllItems()
        {
             
            _context.InventoryItems.AddRange(new List<InventoryItem>
            {
                new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 },
                new InventoryItem {Id = Guid.NewGuid(), Name = "Item2", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 20, ReorderLevel = 15}
            });
            await _context.SaveChangesAsync();

             
            var result = await _controller.GetInventoryItems();

             
            Assert.IsInstanceOf<ActionResult<IEnumerable<InventoryItem>>>(result);
            var items = result.Value.ToList();
            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public async Task GetInventoryItem_ValidId_ReturnsItem()
        {
             
            var item = new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "Test Item",
                Description = "Test Description",
                ProductNumber = "12345",
                QuantityInStock = 10,
                UnitPrice = 99.99m,
                ReorderLevel = 5,
                Supplier = "Test Supplier",
                TaxRate = 23
            };
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

             
            var result = await _controller.GetInventoryItem(item.Id);

             
            Assert.IsInstanceOf<ActionResult<InventoryItem>>(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(item.Id, result.Value.Id);
        }

        [Test]
        public async Task GetInventoryItem_InvalidId_ReturnsNotFound()
        {
             
            var result = await _controller.GetInventoryItem(Guid.NewGuid());

             
            Assert.IsInstanceOf<ActionResult<InventoryItem>>(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetLowStockInventoryItems_ReturnsItemsWithLowStock()
        {
             
   
            var item1 = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345",  UnitPrice = 99.99m,  Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 10, ReorderLevel = 5, };
            var item2 = new InventoryItem { Id = Guid.NewGuid(), Name = "Item2", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 10, ReorderLevel = 8 };
            var item3 = new InventoryItem { Id = Guid.NewGuid(), Name = "Item3", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 12, ReorderLevel = 12 };

            _context.InventoryItems.AddRange(item1, item2, item3);
            await _context.SaveChangesAsync();

             
            var result = await _controller.GetLowStockInventoryItems();

             
            Assert.IsInstanceOf<ActionResult<IEnumerable<InventoryItem>>>(result);
            var items = result.Value.ToList();
   
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(item3.Id, items[0].Id);
        }

        [Test]
        public async Task PostInventoryItem_AddsItemAndReturnsCreatedAtAction()
        {
             
            var newItem = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 };

             
            var result = await _controller.PostInventoryItem(newItem);

             
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.GetInventoryItem), createdResult.ActionName);
            var returnedItem = createdResult.Value as InventoryItem;
            Assert.IsNotNull(returnedItem);
            Assert.AreEqual(newItem.Id, returnedItem.Id);

 
            var itemInDb = await _context.InventoryItems.FindAsync(newItem.Id);
            Assert.IsNotNull(itemInDb);
        }

        [Test]
        public async Task PutInventoryItem_ValidItem_ReturnsNoContent()
        {
             
            var item = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 };
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            item.Name = "UpdatedName";

             
            var result = await _controller.PutInventoryItem(item.Id, item);

             
            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedItem = await _context.InventoryItems.FindAsync(item.Id);
            Assert.AreEqual("UpdatedName", updatedItem.Name);
        }

        [Test]
        public async Task PutInventoryItem_InvalidId_ReturnsBadRequest()
        {
             
            var item = new InventoryItem { Id = Guid.NewGuid(), Name = "Item", QuantityInStock = 5, ReorderLevel = 3 };

             
            var result = await _controller.PutInventoryItem(Guid.NewGuid(), item);

             
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteInventoryItem_ValidId_ReturnsNoContent()
        {
             
            var item = new InventoryItem
            {
                Id = Guid.NewGuid(),
                Name = "Test Item",
                Description = "Test Description",
                ProductNumber = "12345",
                QuantityInStock = 10,
                UnitPrice = 99.99m,
                ReorderLevel = 5,
                Supplier = "Test Supplier",
                TaxRate = 23
            };
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

             
            var result = await _controller.DeleteInventoryItem(item.Id);

             
            Assert.IsInstanceOf<NoContentResult>(result);
            var deletedItem = await _context.InventoryItems.FindAsync(item.Id);
            Assert.IsNull(deletedItem);
        }

        [Test]
        public async Task DeleteInventoryItem_InvalidId_ReturnsNotFound()
        {
             
            var result = await _controller.DeleteInventoryItem(Guid.NewGuid());

             
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task UpdateQuantityInStock_ItemExists_UpdatesQuantity()
        {
             
            var itemId = Guid.NewGuid();
            var item = new InventoryItem
                {
                    Id = itemId,
                Name = "Test Item",
                Description = "Test Description",
                ProductNumber = "12345",
                QuantityInStock = 10,
                UnitPrice = 99.99m,
                ReorderLevel = 5,
                Supplier = "Test Supplier",
                TaxRate = 23
            };
            _context.InventoryItems.Add(item);


            var serviceSchedule = new ServiceSchedule
            {
                Id = Guid.NewGuid(),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { InventoryItemId = itemId, Quantity = 2 }
                }
            };
            _context.ServiceSchedules.Add(serviceSchedule);
            await _context.SaveChangesAsync();

    
            item.QuantityInStock = 1;
            await _context.SaveChangesAsync();


            var result = await _controller.UpdateQuantityInStock(itemId, 3);


            InventoryItem updatedItem;
            if (result.Result is OkObjectResult ok)
            {
                updatedItem = ok.Value as InventoryItem;
            }
            else
            {
                updatedItem = result.Value;
            }
            Assert.IsNotNull(updatedItem);
            Assert.AreEqual(4, updatedItem.QuantityInStock);

        
          
        }

        [Test]
        public async Task GetMissingInventoryItems_ReturnsMissingItems()
        {
             
            var itemId = Guid.NewGuid();
            var item = new InventoryItem
            {
                Id = itemId,
                Name = "MissingItem",
                QuantityInStock = 2,
                ReorderLevel = 5,
                UnitPrice = 10m,
                ProductNumber = "PN123",
                Supplier = "TestSupplier",
                TaxRate = 23,
                Description = "Test Description",
            };
            _context.InventoryItems.Add(item);

 
            var serviceSchedule = new ServiceSchedule
            {
                Id = Guid.NewGuid(),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { InventoryItemId = itemId, Quantity = 5,  }
                }
            };
            _context.ServiceSchedules.Add(serviceSchedule);
            await _context.SaveChangesAsync();

             
            var result = await _controller.GetMissingInventoryItems();

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var missingItems = okResult.Value as IEnumerable<dynamic>;
            Assert.IsNotNull(missingItems);
            var missingItem = missingItems.FirstOrDefault();
            Assert.IsNotNull(missingItem);
            Assert.AreEqual(3, missingItem.MissingStock);
        }
    }
}
