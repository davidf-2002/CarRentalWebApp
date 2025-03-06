using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CarRentalWebApp.Controllers;
using CarRentalWebApp.Repository;
using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalWebApp.Tests.Controllers
{
    [TestClass]
    public class VehicleControllerTest
    {
        private Mock<IVehicleRepo> _mockRepo;
        private VehicleController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IVehicleRepo>();
            _controller = new VehicleController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle> { new Vehicle { Make = "Toyota" }, new Vehicle { Make = "Honda" } };
            _mockRepo.Setup(repo => repo.GetVehicles()).ReturnsAsync(vehicles);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as IEnumerable<Vehicle>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var vehicleViewModel = new VehicleViewModel { Make = "Toyota", Model = "Corolla", Year = 2020, Type = "Sedan" };
            _mockRepo.Setup(repo => repo.CreateVehicle(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(vehicleViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task Delete_ReturnsViewResult_WithVehicle()
        {
            // Arrange
            var vehicle = new Vehicle { VehicleId = 1, Make = "Toyota" };
            _mockRepo.Setup(repo => repo.GetVehicle(1)).ReturnsAsync(vehicle);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(Vehicle));
            var model = viewResult.Model as Vehicle;
            Assert.IsNotNull(model);
            Assert.AreEqual(vehicle.VehicleId, model.VehicleId);
        }

        [TestMethod]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteVehicle(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
    }
}