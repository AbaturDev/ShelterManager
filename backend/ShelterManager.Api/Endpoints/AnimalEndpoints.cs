using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Common.Constants;
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
        group.MapPost("", TestValid)
            .WithRequestValidation<CreateAnimalDto>();
        
        return group;
    }

    private static async Task<Ok<string>> ListAnimals(
        [FromServices] IAnimalService animalService)
    {
        var response = await animalService.ListAnimalsAsync();
        
        return TypedResults.Ok(response);
    }
    
    private static async Task<Ok<string>> TestValid(
        [FromBody] CreateAnimalDto test
        )
    {
        return TypedResults.Ok(test.Name);
    }
}