using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Api.Services;

public class OverdueRentalHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OverdueRentalHostedService> _logger;

    public OverdueRentalHostedService(
        IServiceProvider serviceProvider,
        ILogger<OverdueRentalHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOverdueRentalsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process overdue rental notifications.");
            }

            await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
        }
    }

    private async Task ProcessOverdueRentalsAsync(CancellationToken stoppingToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IBookingRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();
        IChatNotificationService notificationService = scope.ServiceProvider.GetRequiredService<IChatNotificationService>();

        List<int> overdueBookingIds = await bookingRepository.GetOverdueBookingIdsAsync();

        foreach (int bookingId in overdueBookingIds)
        {
            if (stoppingToken.IsCancellationRequested) break;

            await notificationService.NotifyRentalOverdueAsync(bookingId);
            await bookingRepository.MarkOverdueNotifiedAsync(bookingId);
        }
    }
}
