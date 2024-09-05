using System;
using Xunit;

namespace Tekton.Products.Domain.Exception.Tests
{
    public class TecktonExceptionTests
    {
        [Fact]
        public void TecktonException_DefaultConstructor_ShouldSetMessageToNotNull()
        {
            // Act
            var exception = new TecktonException();

            // Assert
            Assert.NotNull(exception.Message);
        }

        [Fact]
        public void TecktonException_MessageConstructor_ShouldSetMessage()
        {
            // Arrange
            var message = "An error occurred.";

            // Act
            var exception = new TecktonException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void TecktonException_CodeAndMessageConstructor_ShouldSetCodeAndMessage()
        {
            // Arrange
            var code = 404;
            var message = "Entity not found.";

            // Act
            var exception = new TecktonException(code, message);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(code, exception.Code);
        }
    }
}
