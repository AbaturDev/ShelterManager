using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Accounts;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute("", apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.LoginRateLimiterName)
            .WithTags(nameof(AccountEndpoints));

        group.MapPost("/login", Login)
            .AllowAnonymous();
        group.MapPost("/register", Register)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicyName)
            .WithRequestValidation<RegisterRequest>();
        group.MapPost("/change-password", ChangePassword)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .WithRequestValidation<ChangePasswordRequest>();
        
        return group;
    }

    private static async Task<Ok<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        [FromServices] IAccountService accountService,
        CancellationToken ct
        )
    {
        var response = await accountService.LoginAsync(request);

        return TypedResults.Ok(response);
    }

    private static async Task<Created> Register(
        [FromBody] RegisterRequest request,
        [FromServices] IAccountService accountService
        )
    {
        await accountService.RegisterAsync(request);
        
        return TypedResults.Created();
    }

    private static async Task<Ok> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        [FromServices] IAccountService accountService)
    {
        await accountService.ChangePasswordAsync(request);
        
        return TypedResults.Ok();
    }
}