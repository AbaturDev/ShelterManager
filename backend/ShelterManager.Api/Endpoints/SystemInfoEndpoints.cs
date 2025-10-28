using ShelterManager.Api.Constants;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Features.SystemInfo;

namespace ShelterManager.Api.Endpoints;

public static class SystemInfoEndpoints
{
    public static IEndpointRouteBuilder MapSystemInfoEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.SystemInfoRoute, apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .RequireAuthorization(AuthorizationPolicies.MustChangePasswordPolicyName)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(SystemInfoEndpoints));

        group.MapGet("/statistics", GetStatistics.GetShelterStatisticsAsync);
        
        return group;
    }
}