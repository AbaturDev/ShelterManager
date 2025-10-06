using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShelterManager.Core.Options;
using ShelterManager.Database.Constants;
using ShelterManager.Database.Entities;

namespace ShelterManager.Database.Seeders.Required;

public static class AdminUserSeeder
{
    public static async Task SeedAsync(UserManager<User> userManager, IOptions<AdminOptions> admin)
    {
        if (await userManager.FindByEmailAsync(admin.Value.Email) is not null)
        {
            return;
        }

        var user = new User
        {
            Name = "Admin",
            Surname = "Admin",
            Email = admin.Value.Email,
            UserName = admin.Value.Email,
            MustChangePassword = false
        };
        
        await userManager.CreateAsync(user, admin.Value.Password);
        await userManager.AddToRoleAsync(user, RoleNames.Admin);
    }
}