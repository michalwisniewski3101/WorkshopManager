using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopManager.api.Controllers;
using WorkshopManager.api.Database;

namespace WorkshopManager.Tests
{
    [TestFixture]
    public class MechanicSpecialtyControllerTests
    {
        private WorkshopContext _context;
        private MechanicSpecialtyController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WorkshopContext>()
                .UseInMemoryDatabase(databaseName: "WorkshopTestDB")
                .Options;

            _context = new WorkshopContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _controller = new MechanicSpecialtyController(_context);
        }

        private MechanicSpecialty CreateTestSpecialty()
        {
            return new MechanicSpecialty
            {
                Id = Guid.NewGuid(),
                SpecialtyName = "Elektryk"
            };
        }

        [Test]
        public async Task GetSpecialties_ReturnsAllSpecialties()
        {
             
            _context.MechanicSpecialties.Add(CreateTestSpecialty());
            _context.MechanicSpecialties.Add(new MechanicSpecialty { Id = Guid.NewGuid(), SpecialtyName = "Lakiernik" });
            await _context.SaveChangesAsync();

             
            var result = await _controller.GetSpecialties();

             
            Assert.IsInstanceOf<ActionResult<IEnumerable<MechanicSpecialty>>>(result);
            Assert.AreEqual(2, result.Value.Count());
        }

        [Test]
        public async Task AddSpecialty_ValidSpecialty_ReturnsOkWithCreatedSpecialty()
        {
             
            var specialty = new MechanicSpecialty { SpecialtyName = "Wulkanizator" };

             
            var result = await _controller.AddSpecialty(specialty);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var createdSpecialty = (result.Result as OkObjectResult).Value as MechanicSpecialty;
            Assert.IsNotNull(createdSpecialty);
            Assert.AreEqual("Wulkanizator", createdSpecialty.SpecialtyName);
            Assert.AreNotEqual(Guid.Empty, createdSpecialty.Id);
        }

        [Test]
        public async Task AddSpecialty_InvalidSpecialty_ReturnsBadRequest()
        {
             
            var specialty = new MechanicSpecialty { SpecialtyName = "" };

             
            var result = await _controller.AddSpecialty(specialty);

             
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("Nazwa specjalnoœci nie mo¿e byæ pusta.", badRequest.Value);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Dispose();
        }
    }
}
