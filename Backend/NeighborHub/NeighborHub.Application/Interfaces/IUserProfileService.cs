using Microsoft.AspNetCore.Http;
using NeighborHub.Application.DTOs.User;

namespace NeighborHub.Application.Interfaces;

public interface IUserProfileService
{
    Task<UserProfileDto> GetProfileAsync(int domainUserId);
    Task<UserProfileDto> UpdateProfilePictureAsync(int domainUserId, IFormFile file);
    Task ChangePasswordAsync(int domainUserId, ChangePasswordDto changePasswordDto);
}
