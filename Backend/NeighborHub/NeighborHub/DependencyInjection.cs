using NeighborHub.Application;
using NeighborHub.Infrastructure;

namespace NeighborHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices( this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices()
                .AddInfrastructureServices(configuration);

                  services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()      // Allow all origins
                          .AllowAnyMethod()      // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader();     // Allow any header
                });
            });

        return services;
    }
}
