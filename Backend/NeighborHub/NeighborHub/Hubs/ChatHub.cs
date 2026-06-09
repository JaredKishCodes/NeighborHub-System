using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NeighborHub.Application.DTOs.Chat;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Api.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    public override async Task OnConnectedAsync()
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, UserGroup(userId.Value));
        int unread = await _chatService.GetUnreadCountAsync(userId.Value);
        await Clients.Caller.SendAsync("UnreadCountUpdated", unread);
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(SendMessageDto dto)
    {
        int? senderId = GetDomainUserId();
        if (!senderId.HasValue) return;

        ChatMessageDto message = await _chatService.SendUserMessageAsync(senderId.Value, dto);
        int recipientUnread = await _chatService.GetUnreadCountAsync(dto.RecipientId);
        await Clients.Group(UserGroup(dto.RecipientId)).SendAsync("UnreadCountUpdated", recipientUnread);
        await Clients.Caller.SendAsync("MessageSent", message);
    }

    public async Task JoinConversation(int otherUserId)
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue) return;

        List<ChatMessageDto> messages = await _chatService.GetConversationAsync(userId.Value, otherUserId);
        await _chatService.MarkConversationReadAsync(userId.Value, otherUserId);
        int unread = await _chatService.GetUnreadCountAsync(userId.Value);
        await Clients.Caller.SendAsync("ConversationLoaded", messages);
        await Clients.Caller.SendAsync("UnreadCountUpdated", unread);
    }

    public async Task MarkRead(int otherUserId)
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue) return;

        await _chatService.MarkConversationReadAsync(userId.Value, otherUserId);
        int unread = await _chatService.GetUnreadCountAsync(userId.Value);
        await Clients.Caller.SendAsync("UnreadCountUpdated", unread);
    }

    private int? GetDomainUserId()
    {
        string? claim = Context.User?.FindFirst("domain_user_id")?.Value;
        return int.TryParse(claim, out int id) ? id : null;
    }

    private static string UserGroup(int userId) => $"user-{userId}";
}
