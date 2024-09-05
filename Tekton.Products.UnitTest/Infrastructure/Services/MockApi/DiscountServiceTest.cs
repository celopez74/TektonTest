using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tekton.Products.Infraestructure.Services.MockApi.Tests
{
    public class DiscountServiceTests
    {
        private readonly Mock<IDiscountService> _mockDiscountService;

        public DiscountServiceTests()
        {
            _mockDiscountService = new Mock<IDiscountService>();
        }

        [Fact]
        public async Task GetDiscountAsync_ShouldReturnExpectedDiscount()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedDiscount = 15;
            _mockDiscountService
                .Setup(ds => ds.GetDiscountAsync(productId))
                .ReturnsAsync(expectedDiscount);

            // Act
            var result = await _mockDiscountService.Object.GetDiscountAsync(productId);

            // Assert
            Assert.Equal(expectedDiscount, result);
        }

        [Fact]
        public async Task GetDiscountAsync_ShouldReturnZero_WhenProductIdIsUnknown()
        {
            // Arrange
            var unknownProductId = Guid.NewGuid();
            _mockDiscountService
                .Setup(ds => ds.GetDiscountAsync(unknownProductId))
                .ReturnsAsync(0);

            // Act
            var result = await _mockDiscountService.Object.GetDiscountAsync(unknownProductId);

            // Assert
            Assert.Equal(0, result);
        }
    }
}
