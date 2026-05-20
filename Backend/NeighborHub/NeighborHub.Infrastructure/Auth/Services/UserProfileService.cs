using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NeighborHub.Application.DTOs.User;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Infrastructure.Auth.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IDomainUserRepository _domainUserRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public UserProfileService(
        IDomainUserRepository domainUserRepository,
        UserManager<AppUser> userManager,
        IWebHostEnvironment env)
    {
        _domainUserRepository = domainUserRepository;
        _userManager = userManager;
        _env = env;
    }

    public async Task<UserProfileDto> GetProfileAsync(int domainUserId)
    {
        DomainUser? domainUser = await _domainUserRepository.GetDomainUserById(domainUserId)
            ?? throw new KeyNotFoundException($"User with ID {domainUserId} was not found.");

        AppUser? appUser = await ResolveAppUserAsync(domainUser);
        return MapProfile(domainUser, appUser);
    }

    public async Task<UserProfileDto> UpdateProfilePictureAsync(int domainUserId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("A profile picture file is required.");
        }

        DomainUser? domainUser = await _domainUserRepository.GetDomainUserById(domainUserId)
            ?? throw new KeyNotFoundException($"User with ID {domainUserId} was not found.");

        string uploadPath = Path.Combine(_env.WebRootPath ?? string.Empty, "profile-images");
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string fullPath = Path.Combine(uploadPath, fileName);

        await using (FileStream stream = new(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        domainUser.ProfilePictureUrl = $"/profile-images/{fileName}";
        await _domainUserRepository.UpdateDomainUserAsync(domainUser);

        AppUser? appUser = await ResolveAppUserAsync(domainUser);
        return MapProfile(domainUser, appUser);
    }

    public async Task ChangePasswordAsync(int domainUserId, ChangePasswordDto changePasswordDto)
    {
        DomainUser? domainUser = await _domainUserRepository.GetDomainUserById(domainUserId)
            ?? throw new KeyNotFoundException($"User with ID {domainUserId} was not found.");

        AppUser? appUser = await ResolveAppUserAsync(domainUser)
            ?? throw new InvalidOperationException("No linked account found for this user.");

        IdentityResult result = await _userManager.ChangePasswordAsync(
            appUser,
            changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }
    }

    private async Task<AppUser?> ResolveAppUserAsync(DomainUser domainUser)
    {
        if (!string.IsNullOrWhiteSpace(domainUser.IdentityId))
        {
            return await _userManager.FindByIdAsync(domainUser.IdentityId);
        }

        return null;
    }

    private static UserProfileDto MapProfile(DomainUser domainUser, AppUser? appUser) => new()
    {
        UserId = domainUser.Id ?? 0,
        FullName = domainUser.FullName ?? $"{appUser?.FirstName} {appUser?.LastName}".Trim(),
        FirstName = appUser?.FirstName ?? string.Empty,
        LastName = appUser?.LastName ?? string.Empty,
        Email = appUser?.Email ?? string.Empty,
        ProfilePictureUrl = domainUser.ProfilePictureUrl,
    };
}
