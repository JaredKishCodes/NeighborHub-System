using NeighborHub.Domain.Enums;

namespace NeighborHub.Application.DTOs.Chat;

public class ChatMessageDto
{
    public int Id { get; set; }
    public int? SenderId { get; set; }
    public string? SenderName { get; set; }
    public int RecipientId { get; set; }
    public int OtherUserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public ChatMessageType MessageType { get; set; }
    public SystemEventType? SystemEventType { get; set; }
    public int? BookingId { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}
