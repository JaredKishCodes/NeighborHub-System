using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure.Repository;

public class ChatRepository(AppDbContext context) : IChatRepository
{
    public async Task<ChatMessage> SaveMessageAsync(ChatMessage message)
    {
        context.ChatMessages.Add(message);
        await context.SaveChangesAsync();
        return message;
    }

    public async Task<List<ChatMessage>> GetConversationAsync(int userId, int otherUserId)
    {
        (int low, int high) = NormalizePair(userId, otherUserId);

        return await context.ChatMessages
            .Include(m => m.Sender)
            .Where(m =>
                m.ParticipantOneId == low &&
                m.ParticipantTwoId == high &&
                (m.SenderId == userId || m.RecipientId == userId || m.MessageType == Domain.Enums.ChatMessageType.System))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<List<int>> GetConversationPartnerIdsAsync(int userId)
    {
        List<ChatMessage> messages = await context.ChatMessages
            .Where(m => m.SenderId == userId || m.RecipientId == userId)
            .ToListAsync();

        return messages
            .Select(m => m.ParticipantOneId == userId ? m.ParticipantTwoId : m.ParticipantOneId)
            .Where(id => id != userId)
            .Distinct()
            .ToList();
    }

    public async Task MarkConversationReadAsync(int userId, int otherUserId)
    {
        (int low, int high) = NormalizePair(userId, otherUserId);

        List<ChatMessage> unread = await context.ChatMessages
            .Where(m =>
                m.ParticipantOneId == low &&
                m.ParticipantTwoId == high &&
                m.RecipientId == userId &&
                !m.IsRead)
            .ToListAsync();

        foreach (ChatMessage message in unread)
        {
            message.IsRead = true;
        }

        await context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await context.ChatMessages.CountAsync(m => m.RecipientId == userId && !m.IsRead);
    }

    private static (int low, int high) NormalizePair(int a, int b) =>
        a < b ? (a, b) : (b, a);

}
