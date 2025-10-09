using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Events;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Mappers;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class EventService : IEventService
{
    private readonly ShelterManagerContext _context;
    private readonly TimeProvider _timeProvider;
    private readonly IUserContextService _userContext;
    
    public EventService(ShelterManagerContext context, IUserContextService userContext, TimeProvider timeProvider)
    {
        _context = context;
        _userContext = userContext;
        _timeProvider = timeProvider;
    }

    public async Task<PaginatedResponse<EventDto>> ListEventsAsync(EventPageQueryFilter queryFilter, CancellationToken ct)
    {
        var query = _context.Events
            .AsNoTracking()
            .ApplyFilters(queryFilter)
            .AsQueryable();
        
        var count = await query.CountAsync(ct);

        var items = await query
            .Select(x => EventMappers.MapToEventDto(x))
            .Paginate(queryFilter.Page, queryFilter.PageSize)
            .ToListAsync(ct);

        return new PaginatedResponse<EventDto>(items, queryFilter.Page, queryFilter.PageSize, count);
    }

    public async Task<EventDto> GetEventAsync(Guid id, CancellationToken ct)
    {
        var eventEntity = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (eventEntity is null)
        {
            throw new NotFoundException("Event not found");
        }

        var eventDto = EventMappers.MapToEventDto(eventEntity);

        return eventDto;
    }

    public async Task<Guid> CreateEventAsync(CreateEventDto dto, CancellationToken ct)
    {
        await ValidateUserExist(dto.UserId, ct);        
        
        var eventEntity = new Event
        {
            Title = dto.Title,
            Description = dto.Description,
            AnimalId = dto.AnimalId,
            Date = dto.Date,
            Cost = MoneyMappers.MapToMoneyEntity(dto.Cost),
            Location = dto.Location,
            UserId = dto.UserId,
            IsDone = false
        };

        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync(ct);

        return eventEntity.Id;
    }

    public async Task DeleteEventAsync(Guid id, CancellationToken ct)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (eventEntity is null)
        {
            throw new NotFoundException("Event not found");
        }

        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateEventAsync(Guid id, UpdateEventDto dto, CancellationToken ct)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (eventEntity is null)
        {
            throw new NotFoundException("Event not found");
        }

        await ValidateUserExist(dto.UserId, ct);        
        
        eventEntity.Title = dto.Title;
        eventEntity.Description = dto.Description;
        eventEntity.Date = dto.Date;
        eventEntity.Cost = MoneyMappers.MapToMoneyEntity(dto.Cost);
        eventEntity.Location = dto.Location;
        eventEntity.UserId = dto.UserId;

        await _context.SaveChangesAsync(ct);
    }

    public async Task EndEventAsync(Guid id, CancellationToken ct)
    {
        var eventEntity = await _context.Events
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (eventEntity is null)
        {
            throw new NotFoundException("Event not found");
        }

        if (eventEntity.IsDone)
        {
            throw new BadRequestException("Event is already set as ended");
        }        
        
        var userId = _userContext.GetCurrentUserId();
        
        eventEntity.IsDone = true;
        eventEntity.CompletedAt = _timeProvider.GetUtcNow();
        eventEntity.CompletedByUserId = userId;

        await _context.SaveChangesAsync(ct);
    }

    private async Task ValidateUserExist(Guid? userId, CancellationToken ct)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);

        if (user is null)
        {
            throw new BadRequestException("User is required. User does not exist in the system");
        }
    }
}