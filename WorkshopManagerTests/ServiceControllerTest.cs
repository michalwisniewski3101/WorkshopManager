using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;
using WorkshopManager.api.Controllers;
using Stripe;

namespace WorkshopManager.Tests
{
    [TestFixture]
    public class ServiceControllerTests
    {
        private Mock<IServiceRepository> _serviceRepositoryMock;
        private ServiceController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _controller = new ServiceController(_serviceRepositoryMock.Object);
        }

        #region GetServices

        [Test]
        public async Task GetServices_ReturnsOkWithServicesList()
        {

            var services = new List<Service>
            {
                new Service { Id = Guid.NewGuid(), ServiceDescription = "Naprawa silnika", ServiceCost = 1500m, ServiceDuration = 120 },
                new Service { Id = Guid.NewGuid(), ServiceDescription = "Wymiana oleju", ServiceCost = 200m, ServiceDuration = 30 }
            };
            _serviceRepositoryMock.Setup(repo => repo.GetServicesAsync())
                .ReturnsAsync(services);


            var result = await _controller.GetServices();

   
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var returnedServices = okResult.Value as IEnumerable<Service>;
            Assert.AreEqual(2, System.Linq.Enumerable.Count(returnedServices));
        }

        #endregion

        #region GetService

        [Test]
        public async Task GetService_ExistingId_ReturnsOkWithService()
        {
             
            var serviceId = Guid.NewGuid();
            var service = new Service
            {
                Id = serviceId,
                ServiceDescription = "Naprawa hamulców",
                ServiceCost = 500m,
                ServiceDuration = 60
            };
            _serviceRepositoryMock.Setup(repo => repo.GetServiceByIdAsync(serviceId))
                .ReturnsAsync(service);

             
            var result = await _controller.GetService(serviceId);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedService = okResult.Value as Service;
            Assert.AreEqual(serviceId, returnedService.Id);
        }

        [Test]
        public async Task GetService_NonExistingId_ReturnsNotFound()
        {
             
            _serviceRepositoryMock.Setup(repo => repo.GetServiceByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Service)null);

             
            var result = await _controller.GetService(Guid.NewGuid());

             
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetServiceSchedule

        [Test]
        public async Task GetServiceSchedule_ExistingId_ReturnsOkWithSchedule()
        {
             
            var scheduleId = Guid.NewGuid();
            var schedule = new ServiceSchedule { Id = scheduleId };
            _serviceRepositoryMock.Setup(repo => repo.GetServiceScheduleByIdAsync(scheduleId))
                .ReturnsAsync(schedule);

             
            var result = await _controller.GetServiceSchedule(scheduleId);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedSchedule = okResult.Value as ServiceSchedule;
            Assert.AreEqual(scheduleId, returnedSchedule.Id);
        }

        [Test]
        public async Task GetServiceSchedule_NonExistingId_ReturnsNotFound()
        {
             
            _serviceRepositoryMock.Setup(repo => repo.GetServiceScheduleByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((ServiceSchedule)null);

             
            var result = await _controller.GetServiceSchedule(Guid.NewGuid());

             
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetServiceSchedulesByOrder

        [Test]
        public async Task GetServiceSchedulesByOrder_ReturnsOkWithSchedules()
        {
             
            var orderId = Guid.NewGuid();
            var schedules = new List<ServiceSchedule>
            {
                new ServiceSchedule { Id = Guid.NewGuid(), OrderId = orderId },
                new ServiceSchedule { Id = Guid.NewGuid(), OrderId = orderId }
            };
            _serviceRepositoryMock.Setup(repo => repo.GetServiceSchedulesByOrderIdAsync(orderId))
                .ReturnsAsync(schedules);

             
            var result = await _controller.GetServiceSchedulesByOrder(orderId);

             
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            var returnedSchedules = okResult.Value as IEnumerable<ServiceSchedule>;
            Assert.AreEqual(2, System.Linq.Enumerable.Count(returnedSchedules));
        }

        #endregion

        #region AddService

        [Test]
        public async Task AddService_ValidService_ReturnsOkWithId()
        {
             
            var service = new Service
            {
                Id = Guid.NewGuid(),
                ServiceDescription = "Wymiana filtrów",
                ServiceCost = 300m,
                ServiceDuration = 45
            };
            _serviceRepositoryMock.Setup(repo => repo.AddServiceAsync(service))
                .ReturnsAsync(service.Id);

             
            var result = await _controller.AddService(service);

             
            Assert.IsInstanceOf<ActionResult<Guid>>(result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(service.Id, okResult.Value);
        }

        [Test]
        public async Task AddService_NullService_ReturnsBadRequest()
        {
             
            var result = await _controller.AddService(null);

             
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region AddServiceSchedule

        [Test]
        public async Task AddServiceSchedule_ValidSchedule_ReturnsOkWithScheduleId()
        {
             
            var orderId = Guid.NewGuid();
            var schedule = new ServiceSchedule { Id = Guid.NewGuid() };
            _serviceRepositoryMock.Setup(repo => repo.AddServiceScheduleAsync(schedule))
                .ReturnsAsync(schedule.Id);

             
            var result = await _controller.AddServiceSchedule(orderId, schedule);

            Assert.AreEqual(orderId, schedule.OrderId);



            Assert.IsInstanceOf<ActionResult<Guid>>(result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(schedule.Id, okResult.Value);
        }

        [Test]
        public async Task AddServiceSchedule_NullSchedule_ReturnsBadRequest()
        {
       
            var orderId = Guid.NewGuid();

     
            var result = await _controller.AddServiceSchedule(orderId, null);

     
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region UpdateServiceScheduleStatus

        [Test]
        public async Task UpdateServiceScheduleStatus_ValidCall_ReturnsNoContent()
        {
          
            var scheduleId = Guid.NewGuid();
            _serviceRepositoryMock.Setup(repo => repo.UpdateServiceScheduleStatusAsync(scheduleId, ServiceStatus.Completed))
                .Returns(Task.CompletedTask);

        
            var result = await _controller.UpdateServiceScheduleStatus(scheduleId, ServiceStatus.Completed);

        
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task UpdateServiceScheduleStatus_Exception_ReturnsStatus500()
        {
         
            var scheduleId = Guid.NewGuid();
            _serviceRepositoryMock.Setup(repo => repo.UpdateServiceScheduleStatusAsync(scheduleId, ServiceStatus.Completed))
                .ThrowsAsync(new Exception("Test exception"));

       
            var result = await _controller.UpdateServiceScheduleStatus(scheduleId, ServiceStatus.Completed);

        
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            StringAssert.Contains("Wyst¹pi³ b³¹d podczas aktualizacji", objectResult.Value.ToString());
        }

        #endregion

        #region DeleteServiceSchedule

        [Test]
        public async Task DeleteServiceSchedule_ValidCall_ReturnsNoContent()
        {
         
            var scheduleId = Guid.NewGuid();
            _serviceRepositoryMock.Setup(repo => repo.DeleteServiceSchedule(scheduleId))
                .ReturnsAsync((ServiceSchedule)null);

           
            var result = await _controller.DeleteServiceSchedule(scheduleId);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteServiceSchedule_Exception_ReturnsStatus500()
        {
         
            var scheduleId = Guid.NewGuid();
            _serviceRepositoryMock.Setup(repo => repo.DeleteServiceSchedule(scheduleId))
                .ThrowsAsync(new Exception("Test exception"));

     
            var result = await _controller.DeleteServiceSchedule(scheduleId);

         
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            StringAssert.Contains("Wyst¹pi³ b³¹d podczas aktualizacji", objectResult.Value.ToString());
        }

        #endregion
    }
}
