using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.DailyTask;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class DailyTaskService : IDailyTaskService
{
    private readonly ShelterManagerContext _context;
    private readonly TimeProvider _timeProvider;
    private readonly IUserContextService _userContext;
    
    public DailyTaskService(ShelterManagerContext context, TimeProvider timeProvider, IUserContextService userContext)
    {
        _context = context;
        _timeProvider = timeProvider;
        _userContext = userContext;
    }
    
    public async Task<DailyTaskDto> GetDailyTaskAsync(Guid animalId, DateOnly date, CancellationToken ct)
    {
        var dailyTask = await _context.DailyTasks
            .AsNoTracking()
            .Include(x => x.Entries)
            .FirstOrDefaultAsync(x => x.Date == date && x.AnimalId == animalId, ct);

        if (dailyTask is null)
        {
            throw new NotFoundException("Daily task not found");
        }

        var dto = new DailyTaskDto
        {
            Id = dailyTask.Id,
            CreatedAt = dailyTask.CreatedAt,
            UpdatedAt = dailyTask.UpdatedAt,
            Date = dailyTask.Date,
            AnimalId = dailyTask.AnimalId,
            Entries = dailyTask.Entries.Select(x => new DailyTaskEntryDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Title = x.Title,
                Description = x.Description,
                IsCompleted = x.IsCompleted,
                CompletedAt = x.CompletedAt,
                DailyTaskId = x.DailyTaskId,
                UserId = x.UserId,
            }).ToList(),
        };

        return dto;
    }

    public async Task AddDailyTaskEntryAsync(Guid animalId, AddDailyTaskEntryDto dto, CancellationToken ct)
    {
        var date = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);
        
        var dailyTask = await _context.DailyTasks
            .Include(x => x.Entries)
            .FirstOrDefaultAsync(x => x.Date == date && x.AnimalId == animalId, ct);

        if (dailyTask is null)
        {
            dailyTask = new DailyTask
            {
                AnimalId = animalId,
                Date = date,
            };
            
            _context.DailyTasks.Add(dailyTask);
        }
        
        dailyTask.Entries.Add(new DailyTaskEntry
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
        });
        
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct)
    {
        var entry = await _context.DailyTaskEntries
            .Include(x => x.DailyTask)
            .FirstOrDefaultAsync(x => x.Id == entryId && x.DailyTask.AnimalId == animalId, ct);
        
        if (entry is null)
        {
            throw new NotFoundException("Daily task entry not found");
        }

        var date = GetTodayDate();
        
        if (entry.DailyTask.Date != date)
        {
            throw new BadRequestException("Can not remove entry from a different day than today");
        }
        
        _context.DailyTaskEntries.Remove(entry);
        await _context.SaveChangesAsync(ct);
    }

    public async Task EndDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct)
    {
        var entry = await _context.DailyTaskEntries
            .Include(x => x.DailyTask)
            .FirstOrDefaultAsync(x => x.Id == entryId && x.DailyTask.AnimalId == animalId, ct);
        
        if (entry is null)
        {
            throw new NotFoundException("Daily task entry not found");
        }

        var date = GetTodayDate();
        
        if (entry.DailyTask.Date != date)
        {
            throw new BadRequestException("Can not modify entry from a different day than today");
        }
        
        if (entry.IsCompleted)
        {
            throw new BadRequestException("Task entry is already completed");
        }

        var userId = _userContext.GetCurrentUserId();
        
        entry.IsCompleted = true;
        entry.CompletedAt = _timeProvider.GetUtcNow();
        entry.UserId = userId;
        
        await _context.SaveChangesAsync(ct);
    }
    
    public async Task AddDefaultDailyTaskEntryAsync(Guid animalId, AddDefaultDailyTaskEntryDto dto, CancellationToken ct)
    {
        if (!await _context.Animals.AnyAsync(x => x.Id == animalId, ct))
        {
            throw new BadRequestException("Can not create default daily task for animal that not exists");
        }

        var defaultEntry = new DailyTaskDefaultEntry
        {
            Title = dto.Title,
            Description = dto.Description,
            AnimalId = animalId,
        };
        
        _context.DailyTaskDefaultEntries.Add(defaultEntry);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveDefaultDailyTaskEntryAsync(Guid animalId, Guid defaultEntryId, CancellationToken ct)
    {
        var defaultEntry = await _context.DailyTaskDefaultEntries
            .FirstOrDefaultAsync(x => x.Id == defaultEntryId && x.AnimalId == animalId, ct);

        if (defaultEntry is null)
        {
            throw new NotFoundException("Default daily task entry not found");
        }
        
        _context.DailyTaskDefaultEntries.Remove(defaultEntry);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<PaginatedResponse<DailyTaskDefaultEntryDto>> GetDefaultDailyTaskEntriesAsync(Guid animalId, PageQueryFilter queryFilter, CancellationToken ct)
    {
        var defaultEntries = _context.DailyTaskDefaultEntries
            .AsNoTracking()
            .Where(x => x.AnimalId == animalId)
            .AsQueryable();

        var totalCount = await defaultEntries.CountAsync(ct);

        var items = await defaultEntries
            .Select(x => new DailyTaskDefaultEntryDto
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Title = x.Title,
                Description = x.Description,
                AnimalId = x.AnimalId,
            })
            .Paginate(queryFilter.Page, queryFilter.PageSize)
            .ToListAsync(ct);
        
        return new PaginatedResponse<DailyTaskDefaultEntryDto>(items, queryFilter.Page, queryFilter.PageSize, totalCount);
    }

    public async Task UpdateDefaultDailyTaskEntryAsync(Guid animalId, Guid defaultEntryId, UpdateDefaultDailyTaskEntryDto dto,
        CancellationToken ct)
    {
        var defaultEntry = await _context.DailyTaskDefaultEntries
            .FirstOrDefaultAsync(x => x.Id == defaultEntryId && x.AnimalId == animalId, ct);

        if (defaultEntry is null)
        {
            throw new NotFoundException("Default daily task entry not found");
        }

        defaultEntry.Title = dto.Title;
        defaultEntry.Description = dto.Description;
        
        await _context.SaveChangesAsync(ct);
    }
    
    private DateOnly GetTodayDate() => DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);
}