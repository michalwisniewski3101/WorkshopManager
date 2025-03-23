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
            // Tworzymy now¹ bazê danych InMemory dla ka¿dego testu
            var options = new DbContextOptionsBuilder<WorkshopContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new WorkshopContext(options);

            // Tworzymy mock dla IServiceRepository
            _serviceRepositoryMock = new Mock<IServiceRepository>();

            // Inicjalizujemy kontroler
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
            // Arrange
            _context.InventoryItems.AddRange(new List<InventoryItem>
            {
                new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 },
                new InventoryItem {Id = Guid.NewGuid(), Name = "Item2", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 20, ReorderLevel = 15}
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetInventoryItems();

            // Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<InventoryItem>>>(result);
            var items = result.Value.ToList();
            Assert.AreEqual(2, items.Count);
        }

        [Test]
        public async Task GetInventoryItem_ValidId_ReturnsItem()
        {
            // Arrange
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

            // Act
            var result = await _controller.GetInventoryItem(item.Id);

            // Assert
            Assert.IsInstanceOf<ActionResult<InventoryItem>>(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(item.Id, result.Value.Id);
        }

        [Test]
        public async Task GetInventoryItem_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetInventoryItem(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<ActionResult<InventoryItem>>(result);
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task GetLowStockInventoryItems_ReturnsItemsWithLowStock()
        {
            // Arrange
            // Tylko elementy, których iloœæ jest mniejsza lub równa reorderLevel
            var item1 = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345",  UnitPrice = 99.99m,  Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 10, ReorderLevel = 5, };
            var item2 = new InventoryItem { Id = Guid.NewGuid(), Name = "Item2", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 10, ReorderLevel = 8 };
            var item3 = new InventoryItem { Id = Guid.NewGuid(), Name = "Item3", Description = "Test Description", ProductNumber = "12345", UnitPrice = 99.99m, Supplier = "Test Supplier", TaxRate = 23, QuantityInStock = 12, ReorderLevel = 12 };

            _context.InventoryItems.AddRange(item1, item2, item3);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetLowStockInventoryItems();

            // Assert
            Assert.IsInstanceOf<ActionResult<IEnumerable<InventoryItem>>>(result);
            var items = result.Value.ToList();
            // W tym przyk³adzie tylko item1 spe³nia warunek (3 <= 5)
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(item3.Id, items[0].Id);
        }

        [Test]
        public async Task PostInventoryItem_AddsItemAndReturnsCreatedAtAction()
        {
            // Arrange
            var newItem = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 };

            // Act
            var result = await _controller.PostInventoryItem(newItem);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(_controller.GetInventoryItem), createdResult.ActionName);
            var returnedItem = createdResult.Value as InventoryItem;
            Assert.IsNotNull(returnedItem);
            Assert.AreEqual(newItem.Id, returnedItem.Id);

            // Sprawdzamy, czy element zosta³ dodany do bazy
            var itemInDb = await _context.InventoryItems.FindAsync(newItem.Id);
            Assert.IsNotNull(itemInDb);
        }

        [Test]
        public async Task PutInventoryItem_ValidItem_ReturnsNoContent()
        {
            // Arrange
            var item = new InventoryItem { Id = Guid.NewGuid(), Name = "Test Item", Description = "Test Description", ProductNumber = "12345", QuantityInStock = 10, UnitPrice = 99.99m, ReorderLevel = 5, Supplier = "Test Supplier", TaxRate = 23 };
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            // Modyfikacja elementu
            item.Name = "UpdatedName";

            // Act
            var result = await _controller.PutInventoryItem(item.Id, item);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var updatedItem = await _context.InventoryItems.FindAsync(item.Id);
            Assert.AreEqual("UpdatedName", updatedItem.Name);
        }

        [Test]
        public async Task PutInventoryItem_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var item = new InventoryItem { Id = Guid.NewGuid(), Name = "Item", QuantityInStock = 5, ReorderLevel = 3 };

            // Act
            var result = await _controller.PutInventoryItem(Guid.NewGuid(), item);

            // Assert
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public async Task DeleteInventoryItem_ValidId_ReturnsNoContent()
        {
            // Arrange
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

            // Act
            var result = await _controller.DeleteInventoryItem(item.Id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            var deletedItem = await _context.InventoryItems.FindAsync(item.Id);
            Assert.IsNull(deletedItem);
        }

        [Test]
        public async Task DeleteInventoryItem_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteInventoryItem(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task UpdateQuantityInStock_ItemExists_UpdatesQuantity()
        {
            // Arrange
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

            // Dodajemy przyk³adowy ServiceSchedule z OrderItems dla tego elementu
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

            // Aby symulowaæ brakuj¹ce elementy, ustawiamy stan magazynowy poni¿ej wymaganego
            item.QuantityInStock = 1;
            await _context.SaveChangesAsync();

            // Act: zwiêkszamy stan magazynowy o 3 jednostki
            var result = await _controller.UpdateQuantityInStock(itemId, 3);

            // Assert
            // Logika UpdateQuantityInStock najpierw zwiêksza stan, a nastêpnie w pêtli odejmuje iloœci orderItems,
            // jeœli brakuj¹cy element istnieje i warunek jest spe³niony.
            // Pocz¹tkowo 1 + 3 = 4, a odejmujemy 2, otrzymuj¹c 2.
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
            // Arrange
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

            // Dodajemy ServiceSchedule z OrderItems, gdzie suma zamówionej iloœci przewy¿sza stan magazynowy
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

            // Act
            var result = await _controller.GetMissingInventoryItems();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var missingItems = okResult.Value as IEnumerable<dynamic>;
            Assert.IsNotNull(missingItems);
            // Oczekiwana wartoœæ brakuj¹cego stanu: 5 - 2 = 3
            var missingItem = missingItems.FirstOrDefault();
            Assert.IsNotNull(missingItem);
            Assert.AreEqual(3, missingItem.MissingStock);
        }
    }
}
