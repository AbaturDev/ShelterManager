using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Common.Constants;
using ShelterManager.Common.Dtos;
using ShelterManager.Common.Utils;
using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class AnimalEndpoints
{
    public static RouteGroupBuilder MapAnimalEndpoints(this IEndpointRouteBuilder route)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.AnimalRoute, 1);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(AnimalEndpoints));

        group.MapGet("", ListAnimals);
        group.MapGet("/{id:guid}", GetAnimal);
        group.MapPost("", CreateAnimal)
            .WithRequestValidation<CreateAnimalDto>();
        
        return group;
    }

    private static async Task<Ok<PaginatedResponse<AnimalDto>>> ListAnimals(
        [AsParameters] PageQueryFilter pageQueryFilter,
        [FromServices] IAnimalService animalService,
        CancellationToken ct)
    {
        var response = await animalService.ListAnimalsAsync(pageQueryFilter, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Ok<AnimalDto>> GetAnimal(
        Guid id,
        [FromServices] IAnimalService animalService,
        CancellationToken ct)
    {
        var response = await animalService.GetAnimalByIdAsync(id, ct);

        return TypedResults.Ok(response);
    }

    private static async Task<Created> CreateAnimal(
        [FromBody] CreateAnimalDto animalDto,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        var id = await animalService.CreateAnimalAsync(animalDto, ct);

        return TypedResults.Created($"/animals/{id}");
    }
}