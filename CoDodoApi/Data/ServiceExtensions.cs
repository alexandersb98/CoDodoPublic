using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Data;

public static class ServiceExtensions
{
    public static IServiceCollection AddConfiguredDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(o =>
        {
            o.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}