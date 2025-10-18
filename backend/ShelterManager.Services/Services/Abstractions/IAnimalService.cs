using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAnimalService
{
    Task<PaginatedResponse<AnimalDto>> ListAnimalsAsync(AnimalPageQueryFilter pageQueryFilter, CancellationToken ct);
    Task<AnimalDto> GetAnimalByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateAnimalAsync(CreateAnimalDto animalDto, CancellationToken ct);
    Task DeleteAnimalAsync(Guid id, CancellationToken ct);
    Task UpdateAnimalAsync(Guid id, UpdateAnimalDto dto, CancellationToken ct);
}