using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ShelterManager.Core.Options;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Seeders.Required;

namespace ShelterManager.Database.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var adminOptions = app.Services.GetRequiredService<IOptions<AdminOptions>>();

        // Required seeders
        await RolesSeeder.SeedAsync(roleManager);
        await AdminUserSeeder.SeedAsync(userManager, adminOptions);
        
        // Development seeders
        if (app.Environment.IsDevelopment())
        {
        }
    }
}