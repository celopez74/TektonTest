using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Tekton.Products.Infrastructure.Modules.Autofac;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Products.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class AutofacSetup
    {
        public static void ConfigureAutofac(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new InfrastructureModule());
        });
    }
    }
}