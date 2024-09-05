using System.Reflection;
using Tekton.Products.Api.Middlewares;
using Tekton.Products.Api.SeedWork;
using Tekton.Products.Api.Startup;
using Tekton.Products.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
    .AddJsonFile(        
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: false,
        reloadOnChange: false);

ServiceSetup.ConfigureServices(builder, Assembly.GetExecutingAssembly());
AutofacSetup.ConfigureAutofac(builder);
var app = builder.Build();
MiddlewareSetup.ConfigureMiddlewares(app);
app.UseMiddleware<RequestTimingMiddleware>();

app.Run();