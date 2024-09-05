using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Domain.SeedWork;
using Tekton.Products.Infraestructure.Services;
using Tekton.Products.Infraestructure.Services.MockApi;
using Tekton.Products.Infrastructure;
using Tekton.Products.Infrastructure.Finder.Products;

public class ProductFinderTests
{
    private readonly ProductFinder _productFinder;
    private readonly TektonContext _context;
    private readonly Mock<IProductStateCacheService> _cacheMock;
    private readonly Mock<IDiscountService> _discountServiceMock;

    public ProductFinderTests()
    {
        // Set up In-Memory DbContext
        var options = new DbContextOptionsBuilder<TektonContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new TektonContext(options);

        // Mock dependencies
        _cacheMock = new Mock<IProductStateCacheService>();
        _discountServiceMock = new Mock<IDiscountService>();

        // Set up ProductFinder
        _productFinder = new ProductFinder(_context, _cacheMock.Object, _discountServiceMock.Object);
    }

    [Fact]
    public async Task GetProductDtoByIdAsync_ShouldReturnProductDto_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {
            Name = "Test Product",
            Price = 100,
            Status = 1,
            Stock = 10,
            Description = "Test description"
        };
        SetProductId(product, productId);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _cacheMock.Setup(c => c.GetProductStatus(It.IsAny<int>())).Returns("Active");
        _discountServiceMock.Setup(d => d.GetDiscountAsync(productId)).ReturnsAsync(10);

        // Act
        var result = await _productFinder.GetProductDtoByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.ProductId);
        Assert.Equal("Active", result.StatusName);
        Assert.Equal(90, result.FinalPrice); // Price after discount
    }

    [Fact]
    public async Task GetProductDtoByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        // Act
        var result = await _productFinder.GetProductDtoByIdAsync(productId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetProductDtoByIdAsync_ShouldReturnCorrectStatusName_WhenStatusIsCached()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {            
            Name = "Test Product",
            Price = 100,
            Status = 2,
            Stock = 10,
            Description = "Test description"
        };
        SetProductId(product, productId);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _cacheMock.Setup(c => c.GetProductStatus(2)).Returns("Inactive");
        _discountServiceMock.Setup(d => d.GetDiscountAsync(productId)).ReturnsAsync(10);

        // Act
        var result = await _productFinder.GetProductDtoByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Inactive", result.StatusName);
    }

    [Fact]
    public async Task GetProductDtoByIdAsync_ShouldCalculateFinalPriceCorrectly_WhenDiscountIsZero()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {            
            Name = "Test Product",
            Price = 100,
            Status = 1,
            Stock = 10,
            Description = "Test description"
        };
        SetProductId(product, productId);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _cacheMock.Setup(c => c.GetProductStatus(It.IsAny<int>())).Returns("Active");
        _discountServiceMock.Setup(d => d.GetDiscountAsync(productId)).ReturnsAsync(0);

        // Act
        var result = await _productFinder.GetProductDtoByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.FinalPrice); // No discount
    }

    [Fact]
    public async Task GetProductDtoByIdAsync_ShouldCalculateFinalPriceCorrectly_WhenDiscountIsApplied()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product
        {            
            Name = "Test Product",
            Price = 200,
            Status = 1,
            Stock = 10,
            Description = "Test description"
        };
        SetProductId(product, productId);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _cacheMock.Setup(c => c.GetProductStatus(It.IsAny<int>())).Returns("Active");
        _discountServiceMock.Setup(d => d.GetDiscountAsync(productId)).ReturnsAsync(20);

        // Act
        var result = await _productFinder.GetProductDtoByIdAsync(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(160, result.FinalPrice); // Price after 20% discount
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
}
