using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.DailyTask;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class DailyTaskEndpoints
{
    public static RouteGroupBuilder MapDailyTaskEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.AnimalRoute, apiVersion);

        var group = route.MapGroup(groupRoute + "/{animalId:guid}/" + ApiRoutes.DailyTaskRoute)
            .RequireAuthorization(AuthorizationPolicies.MustChangePasswordPolicyName)
            .RequireAuthorization(AuthorizationPolicies.UserPolicyName)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(DailyTaskEndpoints));

        group.MapGet("", GetDailyTask);

        group.MapPost($"/{ApiRoutes.DailyTaskEntriesRoute}", AddDailyTaskEntry)
            .WithRequestValidation<AddDailyTaskEntryDto>();
        group.MapDelete($"/{ApiRoutes.DailyTaskEntriesRoute}/{{entryId:guid}}", RemoveDailyTaskEntry);
        group.MapPost($"/{ApiRoutes.DailyTaskEntriesRoute}/{{entryId:guid}}/end", EndDailyTaskEntry);

        group.MapPost($"/{ApiRoutes.DailyTaskDefaultEntriesRoute}", AddDefaultDailyTaskEntry)
            .WithRequestValidation<AddDefaultDailyTaskEntryDto>();
        group.MapPut($"/{ApiRoutes.DailyTaskDefaultEntriesRoute}/{{defaultEntryId:guid}}", UpdateDefaultDailyTaskEntry)
            .WithRequestValidation<UpdateDefaultDailyTaskEntryDto>();
        group.MapDelete($"/{ApiRoutes.DailyTaskDefaultEntriesRoute}/{{defaultEntryId:guid}}", RemoveDefaultDailyTaskEntry);
        group.MapGet($"/{ApiRoutes.DailyTaskDefaultEntriesRoute}", GetDefaultDailyTaskEntries);
        
        return group;
    }

    private static async Task<Ok<DailyTaskDto>> GetDailyTask(
        Guid animalId,
        [FromQuery] DateOnly date,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
        )
    {
        var dailyTask = await dailyTaskService.GetDailyTaskAsync(animalId, date, ct);

        return TypedResults.Ok(dailyTask);
    }
    
    private static async Task<Created> AddDailyTaskEntry(
        Guid animalId,
        [FromBody] AddDailyTaskEntryDto dto,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.AddDailyTaskEntryAsync(animalId, dto, ct);

        return TypedResults.Created();
    }
    
    private static async Task<NoContent> RemoveDailyTaskEntry(
        Guid animalId,
        Guid entryId,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.RemoveDailyTaskEntryAsync(animalId, entryId, ct);

        return TypedResults.NoContent();
    }
    
    private static async Task<Ok> EndDailyTaskEntry(
        Guid animalId,
        Guid entryId,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.EndDailyTaskEntryAsync(animalId, entryId, ct);

        return TypedResults.Ok();
    }
    
    private static async Task<Created> AddDefaultDailyTaskEntry(
        Guid animalId,
        [FromBody] AddDefaultDailyTaskEntryDto dto,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.AddDefaultDailyTaskEntryAsync(animalId, dto, ct);

        return TypedResults.Created();
    }
    
    private static async Task<NoContent> RemoveDefaultDailyTaskEntry(
        Guid animalId,
        Guid defaultEntryId,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.RemoveDefaultDailyTaskEntryAsync(animalId, defaultEntryId, ct);

        return TypedResults.NoContent();
    }
    
    private static async Task<Ok> UpdateDefaultDailyTaskEntry(
        Guid animalId,
        Guid defaultEntryId,
        [FromBody] UpdateDefaultDailyTaskEntryDto dto,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
    )
    {
        await dailyTaskService.UpdateDefaultDailyTaskEntryAsync(animalId, defaultEntryId, dto, ct);

        return TypedResults.Ok();
    }
    
    private static async Task<Ok<ICollection<DailyTaskDefaultEntryDto>>> GetDefaultDailyTaskEntries(
        Guid animalId,
        [FromServices] IDailyTaskService dailyTaskService,
        CancellationToken ct
        )
    {
        var response = await dailyTaskService.GetDefaultDailyTaskEntriesAsync(animalId, ct);
        
        return TypedResults.Ok(response);
    }
}