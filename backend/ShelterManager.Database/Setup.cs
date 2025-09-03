using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Database.Contexts;

namespace ShelterManager.Database;

public static class Setup
{
    public static void AddDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ShelterManagerContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
    }
}