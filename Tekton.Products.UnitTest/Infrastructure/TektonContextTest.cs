using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MediatR;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using System.Reflection;

namespace Tekton.Products.Infrastructure.Tests
{
    public class TektonContextTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TektonContext _context;

        public TektonContextTests()
        {
            _mediatorMock = new Mock<IMediator>();

            var options = new DbContextOptionsBuilder<TektonContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new TektonContext(options);

            // Use reflection to set private _mediator field
            var fieldInfo = typeof(TektonContext).GetField("_mediator", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(_context, _mediatorMock.Object);
        }

        [Fact]
        public async Task SaveEntitiesAsync_ShouldCallDispatchDomainEventsAsync_ThenSaveChangesAsync()
        {
            // Arrange
            var product = new Product
            {
                Name = "Test Product",
                Price = 100.00m,
                Status = 1,
                Stock = 10,
                Description = "Test Description"
            };

            // Add a domain event to the product
            product.AddDomainEvent(new TestDomainEvent());

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _mediatorMock.Setup(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _context.SaveEntitiesAsync(CancellationToken.None);

            // Assert
            Assert.True(result);
            _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        // Test DomainEvent for demonstration purposes
        public class TestDomainEvent : INotification
        {
        }
    }
}
