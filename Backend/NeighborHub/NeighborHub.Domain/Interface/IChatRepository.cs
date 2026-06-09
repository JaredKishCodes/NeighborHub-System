using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;

public interface IChatRepository
{
    Task<ChatMessage> SaveMessageAsync(ChatMessage message);
    Task<List<ChatMessage>> GetConversationAsync(int userId, int otherUserId);
    Task<List<int>> GetConversationPartnerIdsAsync(int userId);
    Task MarkConversationReadAsync(int userId, int otherUserId);
    Task<int> GetUnreadCountAsync(int userId);
}
