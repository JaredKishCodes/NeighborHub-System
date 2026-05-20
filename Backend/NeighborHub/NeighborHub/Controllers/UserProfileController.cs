using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs;
using NeighborHub.Application.DTOs.User;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile(int userId)
    {
        try
        {
            UserProfileDto profile = await _userProfileService.GetProfileAsync(userId);
            return Ok(new ApiResponse<UserProfileDto>
            {
                Success = true,
                Message = "Profile retrieved successfully.",
                Data = profile,
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<UserProfileDto>
            {
                Success = false,
                Message = ex.Message,
                Data = null,
            });
        }
    }

    [HttpPut("{userId}/picture")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateProfilePicture(int userId, IFormFile profilePicture)
    {
        try
        {
            UserProfileDto profile = await _userProfileService.UpdateProfilePictureAsync(userId, profilePicture);
            return Ok(new ApiResponse<UserProfileDto>
            {
                Success = true,
                Message = "Profile picture updated successfully.",
                Data = profile,
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<UserProfileDto>
            {
                Success = false,
                Message = ex.Message,
                Data = null,
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<UserProfileDto>
            {
                Success = false,
                Message = ex.Message,
                Data = null,
            });
        }
    }

    [HttpPut("{userId}/password")]
    public async Task<ActionResult<ApiResponse<bool>>> ChangePassword(int userId, [FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<bool>
            {
                Success = false,
                Message = "Invalid password data.",
                Data = false,
            });
        }

        try
        {
            await _userProfileService.ChangePasswordAsync(userId, changePasswordDto);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Password changed successfully.",
                Data = true,
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse<bool>
            {
                Success = false,
                Message = ex.Message,
                Data = false,
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<bool>
            {
                Success = false,
                Message = ex.Message,
                Data = false,
            });
        }
    }
}
