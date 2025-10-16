using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Dtos.Adoptions;

public record AdoptionPageQueryFilter(Guid? AnimalsId, string? AnimalName, AdoptionStatus? Status, int Page = 1, int PageSize = 10) : PageQueryFilter(Page, PageSize);
