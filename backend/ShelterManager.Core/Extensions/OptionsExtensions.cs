using Microsoft.Extensions.DependencyInjection;

namespace ShelterManager.Core.Extensions;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptionsWithValidation<TOptions>(
        this IServiceCollection services, 
        string sectionName) 
        where TOptions : class
    {
        services.AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        return services;
    }
}