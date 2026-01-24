using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeighborHub.Application.Interfaces.Auth;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Auth.Services;
using NeighborHub.Infrastructure.Persistence;
using NeighborHub.Infrastructure.Repository;

namespace NeighborHub.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext <AppDbContext>(options =>
        {
           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IDomainUserRepository, DomainUserRepository>();

        services.AddScoped<IJwtTokenService,JwtTokenService>();
        return services;
    }
}
