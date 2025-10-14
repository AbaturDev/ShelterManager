using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Adoptions;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class AdoptionEndpoints
{
    public static RouteGroupBuilder MapAdoptionEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.AdoptionRoute, apiVersion);
        
        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .RequireAuthorization(AuthorizationPolicies.MustChangePasswordPolicyName)
            .WithTags(nameof(AdoptionEndpoints));

        group.MapGet("", ListAdoptions);
        group.MapGet("/{id:guid}", GetAdoption);
        group.MapPost("", CreateAdoption)
            .WithRequestValidation<CreateAdoptionDto>();
        group.MapPut("/{id:guid}", UpdateAdoptionStatus)
            .WithRequestValidation<UpdateAdoptionDto>();
        group.MapDelete("/{id:guid}", DeleteAdoption);
        
        return group;
    }

    private static async Task<Ok<PaginatedResponse<AdoptionDto>>> ListAdoptions(
        [AsParameters] AdoptionPageQueryFilter filter,
        [FromServices] IAdoptionService adoptionService,
        CancellationToken ct
        )
    {
        var response = await adoptionService.ListAdoptionsAsync(filter, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<Ok<AdoptionDetailsDto>> GetAdoption(
        Guid id,
        [FromServices] IAdoptionService adoptionService,
        CancellationToken ct
    )
    {
        var response = await adoptionService.GetAdoptionAsync(id, ct);
        
        return TypedResults.Ok(response);
    }

    private static async Task<Created> CreateAdoption(
        [FromBody] CreateAdoptionDto dto,
        [FromServices] IAdoptionService adoptionService,
        CancellationToken ct
        )
    {
        var id = await adoptionService.CreateAdoptionAsync(dto, ct);

        return TypedResults.Created($"{ApiRoutes.AdoptionRoute}/{id}");
    }
    
    private static async Task<NoContent> DeleteAdoption(
        Guid id,
        [FromServices] IAdoptionService adoptionService,
        CancellationToken ct
    )
    {
        await adoptionService.DeleteAdoptionAsync(id, ct);
        
        return TypedResults.NoContent();
    }
    
    private static async Task<Ok> UpdateAdoptionStatus(
        Guid id,
        [FromBody] UpdateAdoptionDto dto,
        [FromServices] IAdoptionService adoptionService,
        CancellationToken ct
    )
    {
        await adoptionService.UpdateAdoptionStatusAsync(id, dto, ct);
        
        return TypedResults.Ok();
    }
}