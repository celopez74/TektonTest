using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Tekton.Products.Api.Application.Commands.Products;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.SeedWork;

public class CreateProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        // Setting up mock UnitOfWork
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _productRepositoryMock.Setup(repo => repo.UnitOfWork).Returns(unitOfWorkMock.Object);

        _handler = new CreateProductCommandHandler(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Product_Fields_When_Successfully_Created()
    {
        // Arrange
        var createProductCommand = new CreateProductCommand
        {
            Name = "Product A",
            Status = 1,
            Stock = 10,
            Description = "Sample description",
            Price = 100.0M
        };

        var product = new Product
        {           
            Name = createProductCommand.Name,
            Status = createProductCommand.Status,
            Stock = createProductCommand.Stock,
            Description = createProductCommand.Description,
            Price = createProductCommand.Price
        };

       
        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>())).Returns(product);

        // Act
        var result = await _handler.Handle(createProductCommand, CancellationToken.None);

        // Assert
        Assert.NotNull(result);            
    }

    [Fact]
    public async Task Handle_Should_Return_False_When_Save_Fails()
    {
        // Arrange
        var createProductCommand = new CreateProductCommand
        {
            Name = "Product B",
            Status = 1,
            Stock = 10,
            Description = "Sample description",
            Price = 100.0M
        };

        // Mock UnitOfWork to return false when SaveEntitiesAsync is called
        _productRepositoryMock.Setup(repo => repo.UnitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(createProductCommand, CancellationToken.None);

        // Assert
        Assert.False((bool)result);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_On_Failure()
    {
        // Arrange
        var createProductCommand = new CreateProductCommand
        {
            Name = "Product C",
            Status = 1,
            Stock = 10,
            Description = "Sample description",
            Price = 100.0M
        };

        // Mock the repository to throw an exception when Add is called
        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>())).Throws(new Exception("Database failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _handler.Handle(createProductCommand, CancellationToken.None));
    }
}
