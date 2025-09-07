using ShelterManager.Api.Endpoints;

namespace ShelterManager.Api.Extensions;

public static class RegisterEndpointsExtensions
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAnimalEndpoints();
    }
}