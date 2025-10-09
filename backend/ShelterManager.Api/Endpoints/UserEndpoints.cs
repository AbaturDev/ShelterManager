using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Users;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.UserRoute, apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(UserEndpoints));

        group.MapGet("/me", GetCurrentUser)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName);
        
        group.MapGet("", ListUsers)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicyName)
            .WithRequestValidation<PageQueryFilter>();

        group.MapGet("/{id:guid}", GetUserById)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicyName);
        
        group.MapDelete("/{id:guid}", DeleteUser)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicyName);
        
        return group;
    }

    private static async Task<Ok<UserDetailsDto>> GetCurrentUser(
        [FromServices] IUserService userService,
        CancellationToken ct
        )
    {
        var response = await userService.GetCurrentUserAsync(ct);
        
        return TypedResults.Ok(response);
    }
    
    private static async Task<Ok<UserDetailsDto>> GetUserById(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken ct
    )
    {
        var response = await userService.GetUserByIdAsync(id, ct);
        
        return TypedResults.Ok(response);
    }
    
    private static async Task<Ok<PaginatedResponse<UserDto>>> ListUsers(
        [AsParameters] PageQueryFilter queryFilter,
        [FromServices] IUserService userService,
        CancellationToken ct
    )
    {
        var response = await userService.ListUsersAsync(queryFilter, ct);
        
        return TypedResults.Ok(response);
    }
    
    private static async Task<NoContent> DeleteUser(
        [FromRoute] Guid id,
        [FromServices] IUserService userService,
        CancellationToken ct
    )
    {
        await userService.SoftDeleteUserAsync(id, ct);
        
        return TypedResults.NoContent();
    }
}