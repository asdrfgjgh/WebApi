
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Controllers;
using WebApi.Repositories;


namespace WebApi.Tests
{
    public class WebApiControllerTests
    {
        private readonly Mock<IRepository> _mockRepository;
        private readonly Mock<ILogger<WebApiController>> _mockLogger;
        private readonly WebApiController _controller;

        public WebApiControllerTests()
        {
            _mockRepository = new Mock<IRepository>();
            _mockLogger = new Mock<ILogger<WebApiController>>();
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            _controller = new WebApiController(_mockRepository.Object, _mockLogger.Object, mockAuthenticationService.Object);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenWebApiDoesNotExist()
        {
            // Arrange
            var webApiId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(webApiId)).ReturnsAsync((WebApi)null);

            // Act
            var result = await _controller.Get(webApiId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Post_CreatesNewWebApi()
        {
            // Arrange
            var newWebApi = new WebApi { name = "New WebApi", ownerUserId = "user3", maxLength = 200, maxHeight = 300 };
            _mockRepository.Setup(repo => repo.AddAsync(It.IsAny<WebApi>())).ReturnsAsync(newWebApi);

            // Act
            var result = await _controller.Post(newWebApi);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<WebApi>(createdAtRouteResult.Value);
            Assert.Equal(newWebApi.name, returnValue.name);
        }

        [Fact]
        public async Task Put_UpdatesExistingWebApi()
        {
            // Arrange
            var webApiId = Guid.NewGuid();
            var existingWebApi = new WebApi { id = webApiId, name = "Existing WebApi", ownerUserId = "user4", maxLength = 250, maxHeight = 350 };
            var updatedWebApi = new WebApi { id = webApiId, name = "Updated WebApi", ownerUserId = "user4", maxLength = 300, maxHeight = 400 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(webApiId)).ReturnsAsync(existingWebApi);
            _mockRepository.Setup(repo => repo.UpdateAsync(updatedWebApi)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(webApiId, updatedWebApi);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<WebApi>(createdAtRouteResult.Value);
            Assert.Equal(updatedWebApi.name, returnValue.name);
        }

        [Fact]
        public async Task Delete_RemovesExistingWebApi()
        {
            // Arrange
            var webApiId = Guid.NewGuid();
            var existingWebApi = new WebApi { id = webApiId, name = "Existing WebApi", ownerUserId = "user5", maxLength = 350, maxHeight = 450 };

            _mockRepository.Setup(repo => repo.GetByIdAsync(webApiId)).ReturnsAsync(existingWebApi);
            _mockRepository.Setup(repo => repo.DeleteAsync(webApiId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(webApiId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}