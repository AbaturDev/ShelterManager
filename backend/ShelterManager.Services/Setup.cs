using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelterManager.Services.Services;
using ShelterManager.Services.Services.Abstractions;
using ShelterManager.Services.Validators;

namespace ShelterManager.Services;

public static class Setup
{
    public static void AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<PageQueryFilterValidator>();
        
        builder.Services.AddScoped<IAnimalService, AnimalService>();
        builder.Services.AddScoped<ISpeciesService, SpeciesService>();
        builder.Services.AddScoped<IBreedService, BreedService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
    }
}