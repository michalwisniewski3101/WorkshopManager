using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WorkshopManager.Tests;
[TestFixture]
public class MechanicControllerTests
{
    private WorkshopContext _context;
    private MechanicController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<WorkshopContext>()
            .UseInMemoryDatabase(databaseName: "WorkshopTestDB")
            .Options;

        _context = new WorkshopContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _controller = new MechanicController(_context);
    }

    private Mechanic CreateTestMechanic()
    {
        return new Mechanic
        {
            Id = Guid.NewGuid(),
            FirstName = "Jan",
            LastName = "Kowalski",
            PhoneNumber = "123456789",
            SpecialtyId = Guid.NewGuid(),
            ExperienceLevel = 5,
            DateJoined = DateTime.UtcNow
        };
    }

    [Test]
    public async Task GetMechanicsList_ReturnsAllMechanics()
    {
         
        _context.Mechanics.Add(CreateTestMechanic());
        _context.Mechanics.Add(CreateTestMechanic());
        await _context.SaveChangesAsync();

         
        var result = await _controller.GetMechanicsList();

         
        Assert.IsInstanceOf<ActionResult<IEnumerable<Mechanic>>>(result);
        Assert.AreEqual(2, result.Value.Count());
    }

    [Test]
    public async Task GetMechanicById_ValidId_ReturnsMechanic()
    {
         
        var mechanic = CreateTestMechanic();
        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();

         
        var result = await _controller.GetMechanic(mechanic.Id);

         
        Assert.IsInstanceOf<ActionResult<Mechanic>>(result);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(mechanic.Id, result.Value.Id);
    }

    [Test]
    public async Task GetMechanicById_InvalidId_ReturnsNotFound()
    {
         
        var result = await _controller.GetMechanic(Guid.NewGuid());

         
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public async Task PostMechanic_ValidMechanic_ReturnsCreatedResponse()
    {
         
        var mechanic = CreateTestMechanic();

         
        var result = await _controller.PostMechanic(mechanic);

         
        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(mechanic.Id, ((Mechanic)createdResult.Value).Id);
    }

    [Test]
    public async Task PutMechanic_ExistingMechanic_UpdatesSuccessfully()
    {
         
        var mechanic = CreateTestMechanic();
        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();

         
        mechanic.ExperienceLevel = 10;
        var result = await _controller.PutMechanic(mechanic);

         
        Assert.IsInstanceOf<NoContentResult>(result);
        var updatedMechanic = await _context.Mechanics.FindAsync(mechanic.Id);
        Assert.AreEqual(10, updatedMechanic.ExperienceLevel);
    }

    [Test]
    public async Task PutMechanic_NonExistingMechanic_ReturnsNotFound()
    {
         
        var mechanic = CreateTestMechanic();

         
        var result = await _controller.PutMechanic(mechanic);

         
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task DeleteMechanic_ValidId_RemovesMechanic()
    {
         
        var mechanic = CreateTestMechanic();
        _context.Mechanics.Add(mechanic);
        await _context.SaveChangesAsync();

         
        var result = await _controller.DeleteMechanic(mechanic.Id);

         
        Assert.IsInstanceOf<NoContentResult>(result);
        Assert.IsNull(await _context.Mechanics.FindAsync(mechanic.Id));
    }

    [Test]
    public async Task DeleteMechanic_InvalidId_ReturnsNotFound()
    {
         
        var result = await _controller.DeleteMechanic(Guid.NewGuid());

         
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Dispose();
    }
}
