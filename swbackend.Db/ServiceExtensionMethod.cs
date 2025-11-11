using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWBackend.DataBase;

namespace swbackend.Db;

public static class ServiceExtensionMethod
{
    public static void AddShootingWorkoutDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SwDbContext>(x =>
        {
            x.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsHistoryTable("migrations_history");
            });
        });
    }
}