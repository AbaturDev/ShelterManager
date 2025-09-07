using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Services.Services;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services;

public static class Setup
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAnimalService, AnimalService>();
    }
}