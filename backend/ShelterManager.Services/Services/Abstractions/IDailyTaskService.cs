using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.DailyTask;

namespace ShelterManager.Services.Services.Abstractions;

public interface IDailyTaskService
{
    Task<DailyTaskDto> GetDailyTaskAsync(Guid animalId, DateOnly date, CancellationToken ct);
    Task AddDailyTaskEntryAsync(Guid animalId, AddDailyTaskEntryDto dto, CancellationToken ct);
    Task RemoveDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct);
    Task EndDailyTaskEntryAsync(Guid animalId, Guid entryId, CancellationToken ct);
    Task AddDefaultDailyTaskEntryAsync(Guid animalId, AddDefaultDailyTaskEntryDto dto, CancellationToken ct);
    Task RemoveDefaultDailyTaskEntryAsync(Guid animalId, Guid defaultEntryId, CancellationToken ct);
    Task<ICollection<DailyTaskDefaultEntryDto>> GetDefaultDailyTaskEntriesAsync(Guid animalId, CancellationToken ct);
    Task UpdateDefaultDailyTaskEntryAsync(Guid animalId, Guid defaultEntryId, UpdateDefaultDailyTaskEntryDto dto, CancellationToken ct);
}