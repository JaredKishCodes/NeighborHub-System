using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs;
using NeighborHub.Application.DTOs.Chat;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("conversations")]
    public async Task<ActionResult<ApiResponse<List<ConversationDto>>>> GetConversations()
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        List<ConversationDto> conversations = await _chatService.GetConversationsAsync(userId.Value);
        return Ok(new ApiResponse<List<ConversationDto>>
        {
            Success = true,
            Message = "Conversations retrieved successfully.",
            Data = conversations,
        });
    }

    [HttpGet("messages/{otherUserId}")]
    public async Task<ActionResult<ApiResponse<List<ChatMessageDto>>>> GetMessages(int otherUserId)
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        List<ChatMessageDto> messages = await _chatService.GetConversationAsync(userId.Value, otherUserId);
        await _chatService.MarkConversationReadAsync(userId.Value, otherUserId);
        return Ok(new ApiResponse<List<ChatMessageDto>>
        {
            Success = true,
            Message = "Messages retrieved successfully.",
            Data = messages,
        });
    }

    [HttpPost("send")]
    public async Task<ActionResult<ApiResponse<ChatMessageDto>>> SendMessage([FromBody] SendMessageDto dto)
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        ChatMessageDto message = await _chatService.SendUserMessageAsync(userId.Value, dto);
        return Ok(new ApiResponse<ChatMessageDto>
        {
            Success = true,
            Message = "Message sent successfully.",
            Data = message,
        });
    }

    [HttpGet("contacts")]
    public async Task<ActionResult<ApiResponse<List<ConversationDto>>>> GetContacts()
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        List<ConversationDto> contacts = await _chatService.GetContactsAsync(userId.Value);
        return Ok(new ApiResponse<List<ConversationDto>>
        {
            Success = true,
            Message = "Contacts retrieved successfully.",
            Data = contacts,
        });
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<ApiResponse<int>>> GetUnreadCount()
    {
        int? userId = GetDomainUserId();
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        int count = await _chatService.GetUnreadCountAsync(userId.Value);
        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Unread count retrieved successfully.",
            Data = count,
        });
    }

    private int? GetDomainUserId()
    {
        string? claim = User.FindFirst("domain_user_id")?.Value;
        return int.TryParse(claim, out int id) ? id : null;
    }
}
