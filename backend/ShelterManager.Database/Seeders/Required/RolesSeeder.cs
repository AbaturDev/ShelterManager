using Microsoft.AspNetCore.Identity;
using ShelterManager.Database.Constants;

namespace ShelterManager.Database.Seeders.Required;

public static class RolesSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        foreach (var role in RoleNames.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}