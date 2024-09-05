using Microsoft.AspNetCore.Mvc;
using Xunit;
using Tekton.Products.Api.Controllers;

namespace Tekton.Products.Api.Tests.Controllers
{
    public class HealthControllerTests
    {
        private readonly HealthController _controller;

        public HealthControllerTests()
        {
            _controller = new HealthController();
        }

        [Fact]
        public async Task GetHealth_ShouldReturnOkResult()
        {
            // Arrange
            var expectedResponse = "OK";

            // Act
            var result = await _controller.GetHealth() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedResponse, result.Value);
        }
    }
}
