using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShelterManager.Api.Constants;
using ShelterManager.Api.Extensions;
using ShelterManager.Api.Utils;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Api.Endpoints;

public static class EventEndpoints
{
    public static RouteGroupBuilder MapEventEndpoints(this IEndpointRouteBuilder route, int apiVersion)
    {
        var groupRoute = ApiRouteBuilder.BuildBaseGroupRoute(ApiRoutes.EventRoute, apiVersion);

        var group = route.MapGroup(groupRoute)
            .RequireRateLimiting(RateLimiters.DefaultRateLimiterName)
            .WithTags(nameof(EventEndpoints));

        group.MapGet("", ListEvents)
            .WithRequestValidation<PageQueryFilter>();
        group.MapGet("/{id:guid}", GetEvent);
        group.MapPost("", CreateEvent)
            .WithRequestValidation<CreateEventDto>();
        group.MapPut("/{id:guid}", UpdateEvent)
            .WithRequestValidation<UpdateEventDto>();
        group.MapDelete("/{id:guid}", DeleteEvent);
        group.MapPost("/{id:guid}/end", EndEvent);
        
        return group;
    }

    private static async Task<Ok<PaginatedResponse<EventDto>>> ListEvents(
        [AsParameters] EventPageQueryFilter pageQueryFilter,
        [FromServices] IEventService eventService,
        CancellationToken ct
        )
    {
        var response = await eventService.ListEventsAsync(pageQueryFilter, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<Ok<EventDto>> GetEvent(
        Guid id,
        [FromServices] IEventService eventService,
        CancellationToken ct
    )
    {
        var response = await eventService.GetEventAsync(id, ct);

        return TypedResults.Ok(response);
    }
    
    private static async Task<Created> CreateEvent(
        [FromBody] CreateEventDto dto,
        [FromServices] IEventService eventService,
        CancellationToken ct
    )
    {
        var id = await eventService.CreateEventAsync(dto, ct);

        return TypedResults.Created($"/{ApiRoutes.EventRoute}/{id}");
    }
    
    private static async Task<NoContent> DeleteEvent(
        Guid id,
        [FromServices] IEventService eventService,
        CancellationToken ct
    )
    {
        await eventService.DeleteEventAsync(id, ct);

        return TypedResults.NoContent();
    }
    
    private static async Task<Ok> UpdateEvent(
        Guid id,
        [FromBody] UpdateEventDto dto,
        [FromServices] IEventService eventService,
        CancellationToken ct
    )
    {
        await eventService.UpdateEventAsync(id, dto, ct);

        return TypedResults.Ok();
    }
    
    private static async Task<Ok> EndEvent(
        Guid id,
        [FromServices] IEventService eventService,
        CancellationToken ct
    )
    {
        await eventService.EndEventAsync(id, ct);

        return TypedResults.Ok();
    }
}