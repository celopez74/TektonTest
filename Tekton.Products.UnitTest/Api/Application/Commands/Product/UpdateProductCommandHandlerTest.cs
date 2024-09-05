using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Api.Application.Commands.Products;
using Tekton.Products.Domain.SeedWork;

public class UpdateProductCommandHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductFinder> _mockProductFinder;
    private readonly UpdateProductCommandHandler _handler;

    public UpdateProductCommandHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductFinder = new Mock<IProductFinder>();
        _handler = new UpdateProductCommandHandler(_mockProductRepository.Object, _mockProductFinder.Object);
    }

    [Fact]
    public async Task Handle_ShouldUpdateProduct_WhenCommandIsValid()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product(); // Assuming Product is your entity

        // Set the Id of the product using reflection
        SetProductId(product, productId);

        // Mock the ProductFinder to return the product
        _mockProductFinder.Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(product);

        // Mock the UnitOfWork
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockUnitOfWork.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Mock the ProductRepository to return the UnitOfWork
        _mockProductRepository.Setup(x => x.UnitOfWork).Returns(mockUnitOfWork.Object);

        // Create the update command
        var updateCommand = new UpdateProductCommand
        {
            Id = productId,
            Name = "Updated Product Name",
            Status = 1,
            Stock = 10,
            Description = "Updated Description",
            Price = 100m
        };

        // Act
        var result = await _handler.Handle(updateCommand, CancellationToken.None);

        // Assert that the product was updated correctly
        _mockProductRepository.Verify(x => x.Update(It.Is<Product>(p => GetProductId(p) == productId)), Times.Once);

        // Assert that SaveEntitiesAsync was called once
        mockUnitOfWork.Verify(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.True((bool)result); // Assuming the result is a boolean indicating success
    }

    [Fact]
    public async Task Handle_ProductNotFound_ThrowsException()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var productFinderMock = new Mock<IProductFinder>();

        productFinderMock.Setup(pf => pf.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null); // Simulating product not found

        var handler = new UpdateProductCommandHandler(productRepositoryMock.Object, productFinderMock.Object);
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Status = 1,
            Stock = 10,
            Description = "Test Description",
            Price = 100.00m
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Contains("Product not found", exception.Message); 
    }

    [Fact]
    public async Task Handle_SaveFails_ReturnsFalse()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var productFinderMock = new Mock<IProductFinder>();
        var product = new Product
        {
            Name = "Test Product",
            Status = 1,
            Stock = 10,
            Description = "Test Description",
            Price = 100.00m
        };

        productFinderMock.Setup(pf => pf.FindByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(product); // Simulating product found

        productRepositoryMock.Setup(pr => pr.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Simulating save fails

        var handler = new UpdateProductCommandHandler(productRepositoryMock.Object, productFinderMock.Object);
        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Status = 1,
            Stock = 10,
            Description = "Test Description",
            Price = 100.00m
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False((bool)result); // Assert that the result is false due to save failure
    }

    // Helper method to set the private/protected Id using reflection
    private void SetProductId(Product product, Guid id)
    {
        var idField = typeof(Entity).GetField("_Id", BindingFlags.Instance | BindingFlags.NonPublic);
        if (idField != null)
        {
            idField.SetValue(product, id);
        }
        else
        {
            throw new Exception("Id field not found in Entity class.");
        }
    }

    // Helper method to get the private/protected Id using reflection
    private Guid GetProductId(Product product)
    {
        var idField = typeof(Entity).GetField("_Id", BindingFlags.Instance | BindingFlags.NonPublic);
        if (idField != null)
        {
            return (Guid)idField.GetValue(product);
        }
        throw new Exception("Id field not found in Entity class.");
    }
}
