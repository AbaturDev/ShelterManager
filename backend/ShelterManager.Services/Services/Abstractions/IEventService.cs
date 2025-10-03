using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;

namespace ShelterManager.Services.Services.Abstractions;

public interface IEventService
{
    Task<PaginatedResponse<EventDto>> ListEventsAsync(CancellationToken ct);
    Task<PaginatedResponse<EventDto>> ListHistoryEventsAsync(CancellationToken ct);
    Task<EventDto> GetEventAsync(CancellationToken ct);
    Task<Guid> CreateEventAsync(CancellationToken ct);
    Task DeleteEventAsync(CancellationToken ct);
    Task UpdateEventAsync(CancellationToken ct);
    Task EndEventAsync(CancellationToken ct);
}