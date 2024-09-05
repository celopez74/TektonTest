using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Tekton.Products.Api.Controllers;

namespace Tekton.Products.Api.Tests.Controllers
{
    public class ValidatorControllerTests
    {
        private readonly ValidatorController _controller;
        private readonly IConfiguration _configuration;

        public ValidatorControllerTests()
        {
            // Build configuration using ConfigurationBuilder and in-memory collection
            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("TektonApisValidationText", "ExpectedValidationText")
                });
            
            _configuration = configurationBuilder.Build();
            
            _controller = new ValidatorController(_configuration);
        }

        [Fact]
        public async Task GetTektonApis_ShouldReturnOkResultWithValidationText()
        {
            // Arrange
            var expectedValidationText = "ExpectedValidationText";

            // Act
            var result = await _controller.GetTektonApis() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedValidationText, result.Value);
        }
    }
}
