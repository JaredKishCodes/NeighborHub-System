using NeighborHub.Application.DTOs.Chat;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Enums;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;

public class ChatNotificationService : IChatNotificationService
{
    private readonly IChatRepository _chatRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IChatRealtimeService _realtimeService;

    public ChatNotificationService(
        IChatRepository chatRepository,
        IBookingRepository bookingRepository,
        IChatRealtimeService realtimeService)
    {
        _chatRepository = chatRepository;
        _bookingRepository = bookingRepository;
        _realtimeService = realtimeService;
    }

    public async Task NotifyBookingRequestedAsync(int bookingId)
    {
        Booking? booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking?.Item == null) return;

        int ownerId = booking.Item.OwnerId;
        int borrowerId = booking.BorrowerId;
        string itemName = booking.Item.Name;
        string borrowerName = booking.Borrower?.FullName ?? "A neighbor";
        string ownerName = booking.Item.Owner?.FullName ?? "the owner";

        await SendSystemMessageAsync(
            ownerId,
            borrowerId,
            bookingId,
            SystemEventType.BookingRequested,
            $"{borrowerName} requested to borrow your {itemName}.");

        await SendSystemMessageAsync(
            borrowerId,
            ownerId,
            bookingId,
            SystemEventType.BookingRequested,
            $"You requested to borrow {itemName} from {ownerName}. Waiting for confirmation.");
    }

    public async Task NotifyBookingConfirmedAsync(int bookingId)
    {
        Booking? booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking?.Item == null) return;

        int ownerId = booking.Item.OwnerId;
        int borrowerId = booking.BorrowerId;
        string itemName = booking.Item.Name;
        string borrowerName = booking.Borrower?.FullName ?? "the borrower";
        string ownerName = booking.Item.Owner?.FullName ?? "the owner";

        await SendSystemMessageAsync(
            borrowerId,
            ownerId,
            bookingId,
            SystemEventType.BookingConfirmed,
            $"{ownerName} confirmed your booking for {itemName}.");

        await SendSystemMessageAsync(
            ownerId,
            borrowerId,
            bookingId,
            SystemEventType.BookingConfirmed,
            $"You confirmed {borrowerName}'s booking for {itemName}.");
    }

    public async Task NotifyRentalOverdueAsync(int bookingId)
    {
        Booking? booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
        if (booking?.Item == null) return;

        int ownerId = booking.Item.OwnerId;
        int borrowerId = booking.BorrowerId;
        string itemName = booking.Item.Name;
        string borrowerName = booking.Borrower?.FullName ?? "the borrower";
        string ownerName = booking.Item.Owner?.FullName ?? "the owner";
        string dueDate = booking.EndDate.ToString("MMM d, yyyy");

        await SendSystemMessageAsync(
            borrowerId,
            ownerId,
            bookingId,
            SystemEventType.RentalOverdue,
            $"Your rental of {itemName} was due on {dueDate}. Please return it to {ownerName}.");

        await SendSystemMessageAsync(
            ownerId,
            borrowerId,
            bookingId,
            SystemEventType.RentalOverdue,
            $"{borrowerName}'s rental of {itemName} is overdue (due {dueDate}).");
    }

    private async Task SendSystemMessageAsync(
        int recipientId,
        int otherUserId,
        int bookingId,
        SystemEventType eventType,
        string content)
    {
        (int low, int high) = recipientId < otherUserId
            ? (recipientId, otherUserId)
            : (otherUserId, recipientId);

        var message = new ChatMessage
        {
            ParticipantOneId = low,
            ParticipantTwoId = high,
            SenderId = null,
            RecipientId = recipientId,
            Content = content,
            MessageType = ChatMessageType.System,
            SystemEventType = eventType,
            BookingId = bookingId,
            SentAt = DateTime.UtcNow,
        };

        ChatMessage saved = await _chatRepository.SaveMessageAsync(message);
        var dto = new ChatMessageDto
        {
            Id = saved.Id,
            SenderId = null,
            SenderName = "System",
            RecipientId = recipientId,
            OtherUserId = otherUserId,
            Content = saved.Content,
            MessageType = ChatMessageType.System,
            SystemEventType = eventType,
            BookingId = bookingId,
            SentAt = saved.SentAt,
            IsRead = false,
        };

        await _realtimeService.PushMessageAsync(recipientId, dto);
        int unread = await _chatRepository.GetUnreadCountAsync(recipientId);
        await _realtimeService.PushUnreadCountAsync(recipientId, unread);
    }
}
