using NeighborHub.Application.DTOs.Chat;

namespace NeighborHub.Application.Interfaces;

public interface IChatRealtimeService
{
    Task PushMessageAsync(int userId, ChatMessageDto message);
    Task PushUnreadCountAsync(int userId, int unreadCount);
}
