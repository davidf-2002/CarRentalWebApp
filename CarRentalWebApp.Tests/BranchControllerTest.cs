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
    public class BranchControllerTest
    {
        private Mock<IBranchRepository> _mockRepo;
        private BranchController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IBranchRepository>();
            _controller = new BranchController(_mockRepo.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfBranches()
        {
            // Arrange
            var branches = new List<Branch> { new Branch { Name = "Branch1" }, new Branch { Name = "Branch2" } };
            _mockRepo.Setup(repo => repo.GetBranches()).ReturnsAsync(branches);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            var model = viewResult.Model as IEnumerable<Branch>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var branchViewModel = new BranchViewModel { Name = "Branch1", City = "City1" };
            _mockRepo.Setup(repo => repo.CreateBranch(It.IsAny<Branch>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(branchViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task Delete_ReturnsViewResult_WithBranch()
        {
            // Arrange
            var branch = new Branch { BranchId = 1, Name = "Branch1" };
            _mockRepo.Setup(repo => repo.GetBranch(1)).ReturnsAsync(branch);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsInstanceOfType(viewResult.Model, typeof(Branch));
            var model = viewResult.Model as Branch;
            Assert.AreEqual(branch.BranchId, model.BranchId);
        }

        [TestMethod]
        public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.DeleteBranch(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
    }
}