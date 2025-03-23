using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkshopManager.api.Controllers;
using WorkshopManager.api.Model;
using WorkshopManager.api.Database;
using static ServiceSchedule;

namespace WorkshopManager.Tests;

[TestFixture]
public class PaymentControllerTests
{
    private WorkshopContext _context;
    private PaymentController _controller;

    [SetUp]
    public void Setup()
    {
 
        var options = new DbContextOptionsBuilder<WorkshopContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new WorkshopContext(options);
        _context.Database.EnsureCreated();

        _controller = new PaymentController(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

  
    [Test]
    public async Task CreateCheckoutSession_OrderNotFound_ReturnsNotFound()
    {
         
        var request = new PaymentRequest
        {
            OrderId = Guid.NewGuid(),
            NeedsInvoice = false
        };

         
        var result = await _controller.CreateCheckoutSession(request);

         
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var notFoundResult = result as NotFoundObjectResult;
        Assert.AreEqual("Nie znaleziono zamówienia", notFoundResult.Value);
    }


}
