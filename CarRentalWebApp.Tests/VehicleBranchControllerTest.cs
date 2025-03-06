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
    public class VehicleBranchControllerTest
    {
        private Mock<IVehicleBranchRepository> _mockRepo;
        private VehicleBranchController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IVehicleBranchRepository>();
            _controller = new VehicleBranchController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfVehicleBranches()
        {
            // Arrange
            var vehicleBranches = new List<VehicleBranch> { new VehicleBranch { VehicleBranchId = 1 }, new VehicleBranch { VehicleBranchId = 2 } };
            _mockRepo.Setup(repo => repo.GetVehicleBranches()).ReturnsAsync(vehicleBranches);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(IEnumerable<VehicleBranch>));
            var model = viewResult.Model as IEnumerable<VehicleBranch>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var vehicleBranch = new VehicleBranch { VehicleBranchId = 1, BranchId = 1, VehicleId = 1, Rate = 100, IsAvailable = true };
            _mockRepo.Setup(repo => repo.CreateVehicleBranch(It.IsAny<VehicleBranch>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(vehicleBranch);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task Delete_ReturnsViewResult_WithVehicleBranch()
        {
            // Arrange
            var vehicleBranch = new VehicleBranch { VehicleBranchId = 1, BranchId = 1, VehicleId = 1, Rate = 100, IsAvailable = true };
            _mockRepo.Setup(repo => repo.GetVehicleBranch(1)).ReturnsAsync(vehicleBranch);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(VehicleBranch));
            var model = viewResult.Model as VehicleBranch;
            Assert.AreEqual(vehicleBranch.VehicleBranchId, model.VehicleBranchId);
        }

        [TestMethod]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteVehicleBranch(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
    }
}