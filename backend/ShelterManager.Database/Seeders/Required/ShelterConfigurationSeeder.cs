using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShelterManager.Core.Options;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;

namespace ShelterManager.Database.Seeders.Required;

public static class ShelterConfigurationSeeder
{
    public static async Task SeedAsync(ShelterManagerContext context, IOptions<ShelterConfigurationOptions> options)
    {
        if (await context.ShelterConfigurations.AnyAsync())
        {
            return;
        }

        var shelterConfiguration = new ShelterConfiguration
        {
            Name = options.Value.Name,
            Street = options.Value.Street,
            City = options.Value.City,
            PostalCode = options.Value.PostalCode,
            PhoneNumber = options.Value.PhoneNumber,
            Email = options.Value.Email,
        };
        
        context.ShelterConfigurations.Add(shelterConfiguration);
        await context.SaveChangesAsync();
    }
}