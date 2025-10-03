using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class EventService : IEventService
{
    public EventService()
    {
        
    }
    
    public Task<PaginatedResponse<EventDto>> ListEventsAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResponse<EventDto>> ListHistoryEventsAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> GetEventAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateEventAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEventAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task UpdateEventAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task EndEventAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}