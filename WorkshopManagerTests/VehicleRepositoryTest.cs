using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkshopManager.api.Repos;

namespace WorkshopManager.Tests
{
    public class VehicleRepositoryTests
    {
        private WorkshopContext _context;
        private VehicleRepository _vehicleRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WorkshopContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WorkshopContext(options);
            _context.Database.EnsureDeleted(); 
            _context.Database.EnsureCreated();

            _vehicleRepository = new VehicleRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllVehiclesAsync_ReturnsAllVehicles()
        {
        
            var vehicles = new List<Vehicle>
                {
                    new Vehicle { Id = Guid.NewGuid(), VIN = "VIN001", Make = "Toyota", Model = "Corolla", Year = 2020, LicensePlate = "XYZ123" },
                    new Vehicle { Id = Guid.NewGuid(), VIN = "VIN002", Make = "Ford", Model = "Focus", Year = 2018, LicensePlate = "ABC789" }
                };
            _context.Vehicles.AddRange(vehicles);
            await _context.SaveChangesAsync();

       
            var result = await _vehicleRepository.GetAllVehiclesAsync();

        
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().VIN, Is.EqualTo("VIN001"));
        }

        [Test]
        public async Task GetVehicleByIdAsync_ReturnsCorrectVehicle()
        {
          
            var vehicle = new Vehicle { Id = Guid.NewGuid(), VIN = "VIN123", Make = "BMW", Model = "X5", Year = 2022, LicensePlate = "DEF456" };
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

        
            var result = await _vehicleRepository.GetVehicleByIdAsync(vehicle.Id);

         
            Assert.That(result, Is.Not.Null);
            Assert.That(result.VIN, Is.EqualTo("VIN123"));
        }

        [Test]
        public async Task GetVehicleByIdAsync_ReturnsNull_WhenVehicleDoesNotExist()
        {
          
            var result = await _vehicleRepository.GetVehicleByIdAsync(Guid.NewGuid());

        
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddVehicleAsync_AddsVehicleSuccessfully()
        {
        
            var vehicle = new Vehicle { Id = Guid.NewGuid(), VIN = "VIN555", Make = "Honda", Model = "Civic", Year = 2019, LicensePlate = "GHI789" };

    
            var result = await _vehicleRepository.AddVehicleAsync(vehicle);
            var retrievedVehicle = await _context.Vehicles.FindAsync(vehicle.Id);

         
            Assert.That(result, Is.Not.Null);
            Assert.That(retrievedVehicle, Is.Not.Null);
            Assert.That(retrievedVehicle.VIN, Is.EqualTo("VIN555"));
        }

        [Test]
        public async Task GetOrAddVehicleAsync_ReturnsExistingVehicleId_WhenVehicleExists()
        {
            var existingVehicle = new Vehicle { Id = Guid.NewGuid(), VIN = "VINEXIST", Make = "Mazda", Model = "CX-5", Year = 2021, LicensePlate = "JKL123" };
            _context.Vehicles.Add(existingVehicle);
            await _context.SaveChangesAsync();

      
            var resultId = await _vehicleRepository.GetOrAddVehicleAsync("VINEXIST", "Mazda", "CX-5", 2021, "JKL123");

            Assert.That(resultId, Is.EqualTo(existingVehicle.Id));
        }

        [Test]
        public async Task GetOrAddVehicleAsync_AddsNewVehicle_WhenVehicleDoesNotExist()
        {

            var newVehicleId = await _vehicleRepository.GetOrAddVehicleAsync("NEWVIN", "Audi", "A4", 2020, "LMN456");

            var retrievedVehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VIN == "NEWVIN");


            Assert.That(retrievedVehicle, Is.Not.Null);
            Assert.That(retrievedVehicle.Id, Is.EqualTo(newVehicleId));
            Assert.That(retrievedVehicle.Make, Is.EqualTo("Audi"));
        }
    }
}
