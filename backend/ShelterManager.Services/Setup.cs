using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Services.Services;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services;

public static class Setup
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(Setup)));
        
        builder.Services.AddScoped<IAnimalService, AnimalService>();
        builder.Services.AddScoped<ISpeciesService, SpeciesService>();
        builder.Services.AddScoped<IBreedService, BreedService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IEventService, EventService>();
    }
}