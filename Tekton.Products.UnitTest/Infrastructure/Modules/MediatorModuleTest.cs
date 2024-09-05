using Autofac;
using Tekton.Products.Infrastructure.Modules.Autofac;
using Tekton.Products.Domain.Models;
using Tekton.Products.Infraestructure.Services;
using MediatR;
using LazyCache;
using Moq;

public class MediatorModuleTests
{
    [Fact]
    public void Load_ShouldRegisterTypesCorrectly()
    {
        // Arrange
        var builder = new ContainerBuilder();
        var module = new MediatorModule();

        // Act
        // Use reflection to call the protected Load method
        var method = typeof(MediatorModule).GetMethod("Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(module, new object[] { builder });

        // Register mock for IAppCache
        var mockCache = new Mock<IAppCache>();
        builder.RegisterInstance(mockCache.Object).As<IAppCache>();

        // Build the container
        var container = builder.Build();

        // Assert
        // Assert that ProductDto is registered
        Assert.True(container.IsRegistered<ProductDto>());

        // Assert that ProductStateCacheService is registered as IProductStateCacheService
        var productStateCacheService = container.Resolve<IProductStateCacheService>();
        Assert.IsType<ProductStateCacheService>(productStateCacheService);

        // Assert that IMediator implementations are registered
        var mediatorInstance = container.Resolve<IMediator>();
        Assert.NotNull(mediatorInstance);

        // Assert that IAppCache is registered
        var cache = container.Resolve<IAppCache>();
        Assert.NotNull(cache);
    }
}
