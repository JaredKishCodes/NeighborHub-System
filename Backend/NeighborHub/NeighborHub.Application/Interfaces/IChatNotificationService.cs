namespace NeighborHub.Application.Interfaces;

public interface IChatNotificationService
{
    Task NotifyBookingRequestedAsync(int bookingId);
    Task NotifyBookingConfirmedAsync(int bookingId);
    Task NotifyRentalOverdueAsync(int bookingId);
}
