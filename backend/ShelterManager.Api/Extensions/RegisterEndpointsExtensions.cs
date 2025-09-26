using Microsoft.Extensions.Options;
using ShelterManager.Api.Endpoints;
using ShelterManager.Core.Options;

namespace ShelterManager.Api.Extensions;

public static class RegisterEndpointsExtensions
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        var apiOptions = app.Services.GetRequiredService<IOptions<ApiOptions>>();

        var apiVersion = apiOptions.Value.Version;
        
        app.MapAnimalEndpoints(apiVersion);
        app.MapBreedEndpoints(apiVersion);
        app.MapSpeciesEndpoints(apiVersion);
    }
}