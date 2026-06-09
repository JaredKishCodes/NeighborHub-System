using NeighborHub.Application.Common;
using NeighborHub.Application.DTOs.Chat;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Enums;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IDomainUserRepository _domainUserRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IChatRealtimeService _realtimeService;

    public ChatService(
        IChatRepository chatRepository,
        IDomainUserRepository domainUserRepository,
        IBookingRepository bookingRepository,
        IChatRealtimeService realtimeService)
    {
        _chatRepository = chatRepository;
        _domainUserRepository = domainUserRepository;
        _bookingRepository = bookingRepository;
        _realtimeService = realtimeService;
    }

    public async Task<ChatMessageDto> SendUserMessageAsync(int senderId, SendMessageDto dto)
    {
        if (senderId == dto.RecipientId)
        {
            throw new InvalidOperationException("You cannot message yourself.");
        }

        DomainUser? recipient = await _domainUserRepository.GetDomainUserById(dto.RecipientId);
        if (recipient == null)
        {
            throw new KeyNotFoundException("Recipient not found.");
        }

        (int low, int high) = NormalizePair(senderId, dto.RecipientId);
        DomainUser? sender = await _domainUserRepository.GetDomainUserById(senderId);

        var message = new ChatMessage
        {
            ParticipantOneId = low,
            ParticipantTwoId = high,
            SenderId = senderId,
            RecipientId = dto.RecipientId,
            Content = dto.Content.Trim(),
            MessageType = ChatMessageType.User,
            SentAt = DateTime.UtcNow,
        };

        ChatMessage saved = await _chatRepository.SaveMessageAsync(message);
        ChatMessageDto dtoResult = MapMessage(saved, senderId, sender?.FullName);

        await _realtimeService.PushMessageAsync(dto.RecipientId, dtoResult);
        await _realtimeService.PushMessageAsync(senderId, dtoResult);

        return dtoResult;
    }

    public async Task<List<ChatMessageDto>> GetConversationAsync(int userId, int otherUserId)
    {
        List<ChatMessage> messages = await _chatRepository.GetConversationAsync(userId, otherUserId);
        return messages.Select(m => MapMessage(m, userId, m.Sender?.FullName)).ToList();
    }

    public async Task<List<ConversationDto>> GetConversationsAsync(int userId)
    {
        List<int> partnerIds = await _chatRepository.GetConversationPartnerIdsAsync(userId);
        var conversations = new List<ConversationDto>();

        foreach (int partnerId in partnerIds)
        {
            DomainUser? partner = await _domainUserRepository.GetDomainUserById(partnerId);
            List<ChatMessage> messages = await _chatRepository.GetConversationAsync(userId, partnerId);
            ChatMessage? last = messages.LastOrDefault();

            conversations.Add(new ConversationDto
            {
                UserId = partnerId,
                FullName = NameHelper.Normalize(partner?.FullName),
                LastMessage = last?.Content,
                LastMessageAt = last?.SentAt,
                UnreadCount = messages.Count(m => m.RecipientId == userId && !m.IsRead),
            });
        }

        return conversations.OrderByDescending(c => c.LastMessageAt).ToList();
    }

    public Task MarkConversationReadAsync(int userId, int otherUserId) =>
        _chatRepository.MarkConversationReadAsync(userId, otherUserId);

    public Task<int> GetUnreadCountAsync(int userId) =>
        _chatRepository.GetUnreadCountAsync(userId);

    public async Task<List<ConversationDto>> GetContactsAsync(int userId)
    {
        List<Booking> borrowings = await _bookingRepository.GetMyBorrowingAsync(userId);
        List<Booking> lendings = await _bookingRepository.GetMyLendingAsync(userId);

        HashSet<int> contactIds = borrowings
            .Select(b => b.Item?.OwnerId ?? 0)
            .Concat(lendings.Select(b => b.BorrowerId))
            .Where(id => id > 0 && id != userId)
            .ToHashSet();

        var contacts = new List<ConversationDto>();
        foreach (int contactId in contactIds)
        {
            DomainUser? user = await _domainUserRepository.GetDomainUserById(contactId);
            contacts.Add(new ConversationDto
            {
                UserId = contactId,
                FullName = NameHelper.Normalize(user?.FullName),
            });
        }

        return contacts.OrderBy(c => c.FullName).ToList();
    }

    private static ChatMessageDto MapMessage(ChatMessage message, int currentUserId, string? senderName) =>
        new()
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderName = message.MessageType == ChatMessageType.System
                ? "System"
                : NameHelper.Normalize(senderName),
            RecipientId = message.RecipientId,
            OtherUserId = message.SenderId == currentUserId
                ? message.RecipientId
                : message.SenderId ?? (message.ParticipantOneId == currentUserId
                    ? message.ParticipantTwoId
                    : message.ParticipantOneId),
            Content = message.Content,
            MessageType = message.MessageType,
            SystemEventType = message.SystemEventType,
            BookingId = message.BookingId,
            SentAt = message.SentAt,
            IsRead = message.IsRead,
        };

    private static (int low, int high) NormalizePair(int a, int b) =>
        a < b ? (a, b) : (b, a);
}
