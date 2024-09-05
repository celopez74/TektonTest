using Autofac;
using Tekton.Products.Infrastructure.Repository;
using Tekton.Products.Domain.AggregatesModel.ProductAggregate;
using Tekton.Products.Infrastructure.Finder.Products;
namespace Tekton.Products.Infrastructure.Modules.Autofac;

public class InfrastructureModule : Module
{

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new HttpClient())
            .SingleInstance();

        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ProductFinder>()
            .As<IProductFinder>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ProductRepository>()
            .As<IProductRepository>()
            .InstancePerLifetimeScope();

       
        builder.Register(ctx => new HttpClient())
            .SingleInstance();

        builder.RegisterType<ProductFinder>()
            .As<IProductFinder>()
            .InstancePerLifetimeScope();
    }
}