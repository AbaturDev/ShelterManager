using ShelterManager.Api.Constants;
using ShelterManager.Common.Constants;
using ShelterManager.Common.Utils;

namespace ShelterManager.Api.Endpoints;

public static class SpeciesEndpoints
{
    public static RouteGroupBuilder MapSpeciesEndpoints(this IEndpointRouteBuilder route)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.SpeciesRoute, 1);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(SpeciesEndpoints));

        
        
        return group;
    }
}