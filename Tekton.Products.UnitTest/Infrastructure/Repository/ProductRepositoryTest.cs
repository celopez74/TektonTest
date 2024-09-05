using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Tekton.Products.Domain.Models;
using Tekton.Products.Infrastructure;
using Tekton.Products.Infrastructure.Repository;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using System.Reflection;

public class ProductRepositoryTests : IDisposable
{
    private readonly TektonContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        // Create an in-memory database context
        var options = new DbContextOptionsBuilder<TektonContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new TektonContext(options);
        _repository = new ProductRepository(_context);

        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void Add_ShouldAddProduct()
    {
        // Arrange
        var product = new Product { Name = "Test Product" , Description = "Descr product"};
        var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        idProperty.SetValue(product, Guid.NewGuid());
        // Act
        var result = _repository.Add(product);
        _context.SaveChanges();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Id, result.Id);
        Assert.Equal(product.Name, result.Name);

        // Verify product is added to the context
        var addedProduct = _context.Products.Find(product.Id);
        Assert.NotNull(addedProduct);
        Assert.Equal(product.Name, addedProduct.Name);
    }

    [Fact]
    public void add_null_product_throws_exception()
    {                
        Assert.Throws<ArgumentNullException>(() => _repository.Add(null));
    }

    [Fact]
    public void Update_ShouldUpdateProduct()
    {
        // Arrange
        var product = new Product { Name = "Test Product" , Description = "Descr product"};
        var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        idProperty.SetValue(product, Guid.NewGuid());

        _context.Products.Add(product);
        _context.SaveChanges();

        // Act
        product.Name = "Updated Product";
        _repository.Update(product);

        // Save changes to the in-memory database
        _context.SaveChanges();

        // Assert
        var updatedProduct = _context.Products.Find(product.Id);
        Assert.NotNull(updatedProduct);
        Assert.Equal("Updated Product", updatedProduct.Name);
    }

    [Fact]
    public void update_null_product_throws_exception()
    {                
        Assert.Throws<ArgumentNullException>(() => _repository.Update(null));
    }

    public void Dispose()
    {
        _context?.Database?.EnsureDeleted();
        _context?.Dispose();
    }
}