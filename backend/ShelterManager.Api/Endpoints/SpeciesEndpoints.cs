using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Common.Constants;
using ShelterManager.Common.Dtos;
using ShelterManager.Common.Utils;
using ShelterManager.Services.Dtos.Breeds;
using ShelterManager.Services.Dtos.Species;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class SpeciesEndpoints
{
    public static RouteGroupBuilder MapSpeciesEndpoints(this IEndpointRouteBuilder route)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.SpeciesRoute, 1);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(SpeciesEndpoints));

        group.MapGet("", ListSpecies)
            .WithRequestValidation<PageQueryFilter>();
        group.MapGet("/{id:guid}", GetSpecies);
        group.MapPost("", CreateSpecies)
            .WithRequestValidation<CreateSpeciesDto>();
        group.MapDelete("{id:guid}", DeleteSpecies);
        
        return group;
    }

    private static async Task<Ok<PaginatedResponse<SpeciesDto>>> ListSpecies(
        [AsParameters] PageQueryFilter pageQueryFilter,
        [FromServices] ISpeciesService speciesService,
        CancellationToken ct
        )
    {
        var response = await speciesService.ListSpeciesAsync(pageQueryFilter, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Ok<SpeciesDto>> GetSpecies(
        Guid id,
        [FromServices] ISpeciesService speciesService,
        CancellationToken ct
        )
    {
        var response = await speciesService.GetSpeciesByIdAsync(id, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<Created> CreateSpecies(
        [FromBody] CreateSpeciesDto dto,
        [FromServices] ISpeciesService speciesService,
        CancellationToken ct
        )
    {
        var id = await speciesService.CreateSpeciesAsync(dto, ct);

        return TypedResults.Created($"/{ApiRoutes.SpeciesRoute}/{id}");
    }

    private static async Task<NoContent> DeleteSpecies(
        Guid id,
        [FromServices] ISpeciesService speciesService,
        CancellationToken ct
        )
    {
        await speciesService.DeleteSpeciesAsync(id, ct);

        return TypedResults.NoContent();
    }
}