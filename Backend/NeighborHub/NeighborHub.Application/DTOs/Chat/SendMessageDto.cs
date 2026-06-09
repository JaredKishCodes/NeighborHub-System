using System.ComponentModel.DataAnnotations;

namespace NeighborHub.Application.DTOs.Chat;

public class SendMessageDto
{
    [Required]
    public int RecipientId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; } = string.Empty;
}
