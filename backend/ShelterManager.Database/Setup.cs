using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;

namespace ShelterManager.Database;

public static class Setup
{
    public static void AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ShelterManagerContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
        
        builder.Services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ShelterManagerContext>()
            .AddDefaultTokenProviders();
    }
}