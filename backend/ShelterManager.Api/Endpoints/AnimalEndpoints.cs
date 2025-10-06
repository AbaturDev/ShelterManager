using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class AnimalEndpoints
{
    public static RouteGroupBuilder MapAnimalEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.AnimalRoute, apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .WithTags(nameof(AnimalEndpoints));

        group.MapGet("", ListAnimals)
            .WithRequestValidation<PageQueryFilter>();
        group.MapGet("/{id:guid}", GetAnimal);
        group.MapPost("", CreateAnimal)
            .WithRequestValidation<CreateAnimalDto>();
        group.MapPut("/{id:guid}", UpdateAnimal)
            .WithRequestValidation<UpdateAnimalDto>();
        group.MapDelete("/{id:guid}", DeleteAnimal);
        
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

        return TypedResults.Created($"/{ApiRoutes.AnimalRoute}/{id}");
    }

    private static async Task<NoContent> DeleteAnimal(
        Guid id,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        await animalService.DeleteAnimalAsync(id, ct);

        return TypedResults.NoContent();
    }

    private static async Task<Ok> UpdateAnimal(
        Guid id,
        [FromBody] UpdateAnimalDto dto,
        [FromServices] IAnimalService animalService,
        CancellationToken ct
    )
    {
        await animalService.UpdateAnimalAsync(id, dto, ct);

        return TypedResults.Ok();
    }
}