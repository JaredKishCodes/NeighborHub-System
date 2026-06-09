namespace NeighborHub.Application.DTOs.Chat;

public class ConversationDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? LastMessage { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
}
