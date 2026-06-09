using NeighborHub.Application.DTOs.Chat;

namespace NeighborHub.Application.Interfaces;

public interface IChatService
{
    Task<ChatMessageDto> SendUserMessageAsync(int senderId, SendMessageDto dto);
    Task<List<ChatMessageDto>> GetConversationAsync(int userId, int otherUserId);
    Task<List<ConversationDto>> GetConversationsAsync(int userId);
    Task MarkConversationReadAsync(int userId, int otherUserId);
    Task<int> GetUnreadCountAsync(int userId);
    Task<List<ConversationDto>> GetContactsAsync(int userId);
}
