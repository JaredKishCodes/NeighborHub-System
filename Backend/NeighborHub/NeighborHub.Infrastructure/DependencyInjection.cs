using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext <AppDbContext>(options =>
        {
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}
