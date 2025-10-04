using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;

namespace ShelterManager.Services.Services.Abstractions;

public interface IEventService
{
    Task<PaginatedResponse<EventDto>> ListEventsAsync(EventPageQueryFilter queryFilter, CancellationToken ct);
    Task<EventDto> GetEventAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateEventAsync(CreateEventDto dto, CancellationToken ct);
    Task DeleteEventAsync(Guid id, CancellationToken ct);
    Task UpdateEventAsync(Guid id, UpdateEventDto dto, CancellationToken ct);
    Task EndEventAsync(Guid id, CancellationToken ct);
}