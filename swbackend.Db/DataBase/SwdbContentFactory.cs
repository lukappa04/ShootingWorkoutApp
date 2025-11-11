using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SWBackend.DataBase;

namespace SWBackend.DataBase;

/// <summary>
/// 
/// </summary>
public class SwDbContextFactory : IDesignTimeDbContextFactory<SwDbContext>
{
    public SwDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        var optionsBuilder = new DbContextOptionsBuilder<SwDbContext>();
        var connectionString = config.GetConnectionString("DefaultConnection");

       optionsBuilder.UseNpgsql(connectionString);

        return new SwDbContext(optionsBuilder.Options);
    }
}
