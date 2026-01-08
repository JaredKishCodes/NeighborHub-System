using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NeighborHub.Application.DTOs.Auth;
using NeighborHub.Application.Interfaces.Auth;

namespace NeighborHub.Infrastructure.Auth.Services;
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Add the 'async' keyword to match Task<string>
    public async Task<string> CreateToken(UserDto user, List<string> roles)
    {
        List<Claim> claims = new()
    {
        // Ensure .ToString() is used if Id is an int
        new Claim("domain_user_id", user.Id.ToString()),
        
        // Use IdentityId (string) for NameIdentifier
        new Claim(ClaimTypes.NameIdentifier, user.Id),


        new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
        new Claim(ClaimTypes.Surname, user.LastName ?? string.Empty)
    };

        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]
            ?? throw new InvalidOperationException("JWT SigningKey not found"));

        SymmetricSecurityKey key = new(keyBytes);
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        // WriteToken returns a string, but because the method is 'async Task', 
        // .NET automatically wraps it in a Task for you.
        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }


}
