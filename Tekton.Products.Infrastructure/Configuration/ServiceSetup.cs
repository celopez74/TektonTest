using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tekton.Products.Infraestructure.Services;
using Tekton.Products.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Tekton.Products.Infraestructure.Services.MockApi;
using System.Diagnostics.CodeAnalysis;

namespace Tekton.Products.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class ServiceSetup
    {
        public static void ConfigureServices(WebApplicationBuilder builder, Assembly assemblies)
        {
            // DbContext
            builder.Services.AddDbContext<TektonContext>(options =>
                options.UseSqlite("Data Source=products.db"));

            // MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(assemblies));

            // Controllers & Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Logging & Services
            builder.Services.AddSingleton<ILoggerService, FileLoggerService>();
            builder.Services.AddSingleton<IProductStateCacheService, ProductStateCacheService>();

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(assemblies);           

            // LazyCache
            builder.Services.AddLazyCache();

            // DiscountService
            builder.Services.AddHttpClient<IDiscountService, DiscountService>();
        }
    }
}