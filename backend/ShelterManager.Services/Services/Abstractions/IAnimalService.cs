using ShelterManager.Common.Dtos;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAnimalService
{
    Task<PaginatedResponse<AnimalDto>> ListAnimalsAsync(PageQueryFilter pageQueryFilter, CancellationToken ct);
    Task<AnimalDto> GetAnimalByIdAsync(Guid id, CancellationToken ct);
    Task<Guid> CreateAnimalAsync(CreateAnimalDto animalDto, CancellationToken ct);
    //delete
    //update
}