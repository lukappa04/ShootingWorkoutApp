using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SWBackend.DataBase;

namespace SWBackend.DataBase;

public class SWDbContextFactory : IDesignTimeDbContextFactory<SWDbContext>
{
    public SWDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        var optionsBuilder = new DbContextOptionsBuilder<SWDbContext>();
        var connectionString = config.GetConnectionString("DefaultConnection");

       optionsBuilder.UseNpgsql(connectionString);

        return new SWDbContext(optionsBuilder.Options);
    }
}
