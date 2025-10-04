using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Dtos.Events;

public record EventPageQueryFilter(Guid[]? AnimalIds, bool? IsDone, int Page = 1, int PageSize = 10)
    : PageQueryFilter(Page, PageSize);