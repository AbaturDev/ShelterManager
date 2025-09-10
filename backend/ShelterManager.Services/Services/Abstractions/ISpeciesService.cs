using ShelterManager.Common.Dtos;
using ShelterManager.Services.Dtos.Species;

namespace ShelterManager.Services.Services.Abstractions;

public interface ISpeciesService
{
    Task<PaginatedResponse<SpeciesDto>> ListSpeciesAsync(PageQueryFilter pageQueryFilter, CancellationToken ct);
    Task<SpeciesDto> GetSpeciesByIdAsync(Guid id, CancellationToken ct);
    Task CreateSpeciesAsync(CreateSpeciesDto dto, CancellationToken ct);
    Task DeleteSpeciesAsync(Guid id, CancellationToken ct);
}