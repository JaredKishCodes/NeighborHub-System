using NeighborHub.Domain.Enums;

namespace NeighborHub.Domain.Entities;

public class ChatMessage
{
    public int Id { get; set; }
    public int ParticipantOneId { get; set; }
    public int ParticipantTwoId { get; set; }
    public int? SenderId { get; set; }
    public int RecipientId { get; set; }
    public string Content { get; set; } = string.Empty;
    public ChatMessageType MessageType { get; set; }
    public SystemEventType? SystemEventType { get; set; }
    public int? BookingId { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }

    public DomainUser? Sender { get; set; }
    public DomainUser? Recipient { get; set; }
}
