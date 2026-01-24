using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NeighborHub.Application.DTOs.Auth;
using NeighborHub.Application.Interfaces.Auth;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Infrastructure.Auth.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
  
    private readonly IDomainUserRepository _domainUserRepository;


    public AuthService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService, IDomainUserRepository domainUserRepository)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _domainUserRepository = domainUserRepository;
   
    }

    public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
    {
        AppUser? user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }


        IList<string> roles = await _userManager.GetRolesAsync(user);
        string? role = roles.FirstOrDefault();

        string token = await _jwtTokenService.CreateTokenAsync(new UserDto
        {
            Id = user.Id, // adjust if Id is Guid or string
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
        }, roles.ToList());

        var response = new LoginResponse
        {
            Token = token,
            Role = role,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
        };


        return response;

    }


    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        // Check if user already exists
        AppUser? existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            throw new ApplicationException("User already exists with this email.");
        }

        // Create user
        var newUser = new AppUser
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            StreetAddress = registerDto.StreetAddress,
            City = registerDto.City,
            Baranggay = registerDto.Baranggay
        };

        IdentityResult createdUser = await _userManager.CreateAsync(newUser, registerDto.Password);
        if (!createdUser.Succeeded)
        {
            string errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
            throw new ApplicationException($"User creation failed: {errors}");
        }

        if (createdUser.Succeeded)
        {
            // 2. Map to the Domain User (Profile)
            var domainUser = new DomainUser
            {
                IdentityId = newUser.Id, 
                FullName = registerDto.FirstName + " " + registerDto.LastName, // Assuming FullName is a combination of FirstName and LastName
            
            };

            // 3. Save to your NeighborHub domain database
            await _domainUserRepository.CreateDomainUserAsync(domainUser);

            int usersCount = await _userManager.Users.CountAsync();

            if (usersCount == 1)
            {
                await _userManager.AddToRoleAsync(newUser, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, "User");
            }

            IList<string> roles = await _userManager.GetRolesAsync(newUser);
            // Generate JWT
            string token = await _jwtTokenService.CreateTokenAsync(new UserDto
            {
                Id = newUser.Id, // Change this if your AppUser.Id is string or Guid
                Email = newUser.Email!,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
            }, roles.ToList());

            return new AuthResponseDto
            {
                Token = token,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
            };
        }

        throw new ApplicationException("Unexpected error occurred during user registration.");
    }
}
