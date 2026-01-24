using Microsoft.AspNetCore.Identity;
using NeighborHub.Infrastructure.Auth;
using NeighborHub.Domain.Entities; // Added for DomainUser
using NeighborHub.Domain.Interface; // Added for Repository

namespace NeighborHub.Api;

public static class SeedRoleData
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // IMPORTANT: Get your Domain User repository to keep data in sync
        IDomainUserRepository domainUserRepository = serviceProvider.GetRequiredService<IDomainUserRepository>();

        // Ensure Roles Exist
        string[] roles = new string[] { "Admin", "User" };
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        string adminEmail = configuration["AdminUser:Email"];
        string adminPassword = configuration["AdminUser:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
           
            return;
        }

        AppUser? adminUser = await userManager.FindByEmailAsync(adminEmail);

        // Helper: Try to parse string Id to int?
        static int? ParseUserId(string idStr)
        {
            if (int.TryParse(idStr, out int id))
            {
                return id;
            }

            return null;
        }

        if (adminUser == null)
        {
            var user = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            IdentityResult result = await userManager.CreateAsync(user, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");

                // --- CRITICAL NEIGHBORHUB STEP ---
                // Seed the DomainUser record so the Admin can own items
                var domainAdmin = new DomainUser
                {
                    Id = ParseUserId(user.Id), // Link to Identity ID as int?
                    FullName = "System Administrator",
                };

                await domainUserRepository.CreateDomainUserAsync(domainAdmin);

            }
        }
        else
        {
            // Ensure the existing admin has the role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Optional: Check if Domain record exists, create if missing
            int? adminDomainId = ParseUserId(adminUser.Id);
            if (adminDomainId.HasValue)
            {
                DomainUser? existingDomainUser = await domainUserRepository.GetDomainUserById(adminDomainId.Value);
                if (existingDomainUser == null)
                {
                    var domainAdmin = new DomainUser
                    {
                        Id = adminDomainId,
                        FullName = "System Administrator",
                    };
                    await domainUserRepository.CreateDomainUserAsync(domainAdmin);
                }
            }

        }
    }
}
