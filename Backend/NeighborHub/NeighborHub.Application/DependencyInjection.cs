using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NeighborHub.Application.Services;
using NeighborHub.Application.Interfaces;


namespace NeighborHub.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        return services;
    }
}
