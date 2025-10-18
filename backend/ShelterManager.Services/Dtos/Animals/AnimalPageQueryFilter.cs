using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Dtos.Animals;

public record AnimalPageQueryFilter(string? Name, Sex? Sex, AnimalStatus? Status, int Page = 1, int PageSize = 10) : PageQueryFilter(Page, PageSize);
