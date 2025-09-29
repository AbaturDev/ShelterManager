using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Breeds;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class BreedEndpoints
{
    public static RouteGroupBuilder MapBreedEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.SpeciesRoute, apiVersion);

        var group = route.MapGroup(groupRoute + "/{speciesId:guid}/" + ApiRoutes.BreedRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(BreedEndpoints));

        group.MapGet("/{id:guid}", GetBreed);
        group.MapDelete("/{id:guid}", DeleteBreed);
        group.MapGet("", ListBreeds)
            .WithRequestValidation<PageQueryFilter>();
        group.MapPost("", CreateBreed)
            .WithRequestValidation<CreateBreedDto>();
        
        return group;
    }

    private static async Task<Ok<BreedDto>> GetBreed(
        Guid id,
        Guid speciesId,
        [FromServices] IBreedService breedService,
        CancellationToken ct
        )
    {
        var response = await breedService.GetBreedByIdAsync(id, speciesId, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<NoContent> DeleteBreed(
        Guid id,
        Guid speciesId,
        [FromServices] IBreedService breedService,
        CancellationToken ct
    )
    {
        await breedService.DeleteBreedAsync(id, speciesId, ct);

        return TypedResults.NoContent();
    }
    
    private static async Task<Ok<PaginatedResponse<BreedDto>>> ListBreeds(
        Guid speciesId,
        [AsParameters] PageQueryFilter pageQueryFilter,
        [FromServices] IBreedService breedService,
        CancellationToken ct
    )
    {
        var response = await breedService.ListBreedsAsync(pageQueryFilter, speciesId, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Created> CreateBreed(
        Guid speciesId,
        [FromBody] CreateBreedDto dto,
        [FromServices] IBreedService breedService,
        CancellationToken ct
    )
    {
        var id = await breedService.CreateBreedAsync(dto, speciesId, ct);

        return TypedResults.Created($"{ApiRoutes.SpeciesRoute}/{speciesId}/{ApiRoutes.BreedRoute}/{id}");
    }
}