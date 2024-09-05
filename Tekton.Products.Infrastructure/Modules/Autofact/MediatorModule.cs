using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Module = Autofac.Module;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tekton.Products.Domain.Models;
using Tekton.Products.Infraestructure.Services;
using LazyCache;


namespace Tekton.Products.Infrastructure.Modules.Autofac;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        builder.RegisterType<ProductDto>().AsSelf();
        builder.RegisterType<ProductStateCacheService>()
            .As<IProductStateCacheService>()
            .SingleInstance();

        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        var services = new ServiceCollection();

        builder.Populate(services);
    }
}