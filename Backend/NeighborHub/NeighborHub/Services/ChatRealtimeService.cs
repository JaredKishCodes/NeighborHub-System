using Microsoft.AspNetCore.SignalR;
using NeighborHub.Api.Hubs;
using NeighborHub.Application.DTOs.Chat;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Api.Services;

public class ChatRealtimeService(IHubContext<ChatHub> hubContext) : IChatRealtimeService
{
    public Task PushMessageAsync(int userId, ChatMessageDto message) =>
        hubContext.Clients.Group(UserGroup(userId)).SendAsync("ReceiveMessage", message);

    public Task PushUnreadCountAsync(int userId, int unreadCount) =>
        hubContext.Clients.Group(UserGroup(userId)).SendAsync("UnreadCountUpdated", unreadCount);

    private static string UserGroup(int userId) => $"user-{userId}";
}
