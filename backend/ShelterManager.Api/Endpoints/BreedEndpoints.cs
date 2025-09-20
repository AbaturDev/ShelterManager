using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Common.Constants;
using ShelterManager.Common.Utils;
using ShelterManager.Services.Dtos.Breeds;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class BreedEndpoints
{
    public static RouteGroupBuilder MapBreedEndpoints(this IEndpointRouteBuilder route)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.BreedRoute, 1);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(BreedEndpoints));

        group.MapGet("/{id:guid}", GetBreed);
        group.MapDelete("/{id:guid}", DeleteBreed);
        
        return group;
    }

    private static async Task<Ok<BreedDto>> GetBreed(
        Guid id,
        [FromServices] IBreedService breedService,
        CancellationToken ct
        )
    {
        var response = await breedService.GetBreedByIdAsync(id, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<NoContent> DeleteBreed(
        Guid id,
        [FromServices] IBreedService breedService,
        CancellationToken ct
    )
    {
        await breedService.DeleteBreedAsync(id, ct);

        return TypedResults.NoContent();
    }
}