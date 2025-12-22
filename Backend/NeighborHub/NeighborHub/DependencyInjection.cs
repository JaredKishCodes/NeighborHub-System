using NeighborHub.Application;
using NeighborHub.Infrastructure;

namespace NeighborHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices( this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices()
                .AddInfrastructureServices(configuration);

        return services;
    }
}
