using ShelterManager.Common.Dtos;
using ShelterManager.Services.Dtos.Breeds;

namespace ShelterManager.Services.Services.Abstractions;

public interface IBreedService
{
    Task<PaginatedResponse<BreedDto>> ListBreedsAsync(PageQueryFilter pageQueryFilter, Guid speciesId, CancellationToken ct);
    Task<BreedDto> GetBreedByIdAsync(Guid id, CancellationToken ct);
    Task CreateBreedAsync(CreateBreedDto dto, Guid speciesId, CancellationToken ct);
    Task DeleteBreedAsync(Guid id, CancellationToken ct);
}