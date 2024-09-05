using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Tekton.Products.Api.Application.Queries.Products;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.Models;
using System.Reflection;

public class GetProductQueryHandlerTests
{
    private readonly Mock<IProductFinder> _productFinderMock;
    private readonly GetProductQueryHandler _handler;

    public GetProductQueryHandlerTests()
    {
        _productFinderMock = new Mock<IProductFinder>();
        _handler = new GetProductQueryHandler(_productFinderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProductDto = new ProductDto
        {
            Name = "Test Product",
            Status = 1,
            Stock = 10,
            Description = "Test Description",
            Price = 100.0m
        };

        var idProperty = typeof(GetProductQuery).GetProperty("id", BindingFlags.Instance | BindingFlags.NonPublic);
        idProperty?.SetValue(expectedProductDto, productId);

        _productFinderMock.Setup(finder => finder.GetProductDtoByIdAsync(productId))
            .ReturnsAsync(expectedProductDto);

        var query = new GetProductQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();        
        result.Name.Should().Be(expectedProductDto.Name);
        result.Status.Should().Be(expectedProductDto.Status);
        result.Stock.Should().Be(expectedProductDto.Stock);
        result.Description.Should().Be(expectedProductDto.Description);
        result.Price.Should().Be(expectedProductDto.Price);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productFinderMock.Setup(finder => finder.GetProductDtoByIdAsync(productId))
            .ReturnsAsync((ProductDto)null);

        var query = new GetProductQuery(productId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenProductFinderThrowsException()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productFinderMock.Setup(finder => finder.GetProductDtoByIdAsync(productId))
            .ThrowsAsync(new InvalidOperationException("Database is down"));

        var query = new GetProductQuery(productId);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Database is down");
    }
}
