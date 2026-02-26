using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NeighborHub.Application;
using NeighborHub.Infrastructure;
using NeighborHub.Infrastructure.Auth;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices( this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices()
                .AddInfrastructureServices(configuration);

                  services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()      // Allow all origins
                          .AllowAnyMethod()      // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader();     // Allow any header
                });
            });

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = configuration["JWT:Issuer"],
             ValidAudience = configuration["JWT:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(
                 System.Text.Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]))
         });

        return services;
    }
}
