using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WorkshopManager.api.Repos.Interfaces;
using WorkshopManager.api.Controllers;

namespace WorkshopManager.Tests
{
    public class VehicleControllerTests
    {
        private Mock<IVehicleRepository> _vehicleRepositoryMock;
        private VehicleController _controller;

        [SetUp]
        public void Setup()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _controller = new VehicleController(_vehicleRepositoryMock.Object);
        }

        [Test]
        public async Task GetVehicles_ReturnsListOfVehicles()
        {
        
            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = Guid.NewGuid(), VIN = "VIN001", Make = "Toyota", Model = "Corolla", Year = 2020, LicensePlate = "XYZ123" },
                new Vehicle { Id = Guid.NewGuid(), VIN = "VIN002", Make = "Ford", Model = "Focus", Year = 2018, LicensePlate = "ABC789" }
            };

            _vehicleRepositoryMock.Setup(repo => repo.GetAllVehiclesAsync())
                .ReturnsAsync(vehicles);

        
            var result = await _controller.GetVehicles();
            var okResult = result.Result as OkObjectResult;
            var returnedVehicles = okResult.Value as IEnumerable<Vehicle>;

      
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(returnedVehicles.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetVehicle_ReturnsVehicle_WhenExists()
        {
     
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Id = vehicleId, VIN = "VIN123", Make = "BMW", Model = "X5", Year = 2022, LicensePlate = "DEF456" };

            _vehicleRepositoryMock.Setup(repo => repo.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);

     
            var result = await _controller.GetVehicle(vehicleId);
            var okResult = result.Result as OkObjectResult;
            var returnedVehicle = okResult.Value as Vehicle;

       
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(returnedVehicle.VIN, Is.EqualTo("VIN123"));
        }

        [Test]
        public async Task GetVehicle_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
 
            _vehicleRepositoryMock.Setup(repo => repo.GetVehicleByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Vehicle)null);

    
            var result = await _controller.GetVehicle(Guid.NewGuid());

       
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task PostVehicle_CreatesNewVehicle()
        {
    
            var newVehicle = new Vehicle { Id = Guid.NewGuid(), VIN = "VIN555", Make = "Honda", Model = "Civic", Year = 2019, LicensePlate = "GHI789" };

            _vehicleRepositoryMock.Setup(repo => repo.AddVehicleAsync(It.IsAny<Vehicle>()))
                .ReturnsAsync(newVehicle);

   
            var result = await _controller.PostVehicle(newVehicle);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            var createdVehicle = createdAtActionResult.Value as Vehicle;

     
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdVehicle.VIN, Is.EqualTo("VIN555"));
        }
    }
}
