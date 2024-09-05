using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Tekton.Products.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TektonContext>
{
    public TektonContext CreateDbContext(string[] args)
    {
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)           
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TektonContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");        

        optionsBuilder.UseSqlite(connectionString);

        return new TektonContext(optionsBuilder.Options);   
       
    }
}