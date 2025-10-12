using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.DailyTask;

namespace ShelterManager.Services.Services.Abstractions;

public interface IDailyTaskService
{
    Task<DailyTaskDto> GetDailyTaskAsync(Guid animalId, DateOnly date, CancellationToken ct);
    Task AddDailyTaskEntryAsync(Guid animalId, AddDailyTaskEntryDto dto, CancellationToken ct);
    Task RemoveDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct);
    Task EndDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct);
    Task AddDefaultDailyTaskAsync(Guid animalId, AddDefaultDailyTaskEntryDto dto, CancellationToken ct);
    Task RemoveDefaultDailyTaskAsync(Guid animalId, Guid defaultEntryId, CancellationToken ct);
    Task<PaginatedResponse<DailyTaskDefaultEntryDto>> GetDefaultDailyTaskAsync(Guid animalId, PageQueryFilter queryFilter, CancellationToken ct);
    Task UpdateDefaultDailyTaskAsync(Guid animalId, Guid defaultEntryId, UpdateDefaultDailyTaskEntryDto dto, CancellationToken ct);
}