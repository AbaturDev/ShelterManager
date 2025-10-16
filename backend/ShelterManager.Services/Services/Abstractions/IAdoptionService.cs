using ShelterManager.Services.Dtos.Adoptions;
using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAdoptionService
{
    Task<AdoptionDetailsDto> GetAdoptionAsync(Guid id, CancellationToken ct);
    Task<PaginatedResponse<AdoptionDto>> ListAdoptionsAsync(AdoptionPageQueryFilter filter, CancellationToken ct);
    Task<Guid> CreateAdoptionAsync(CreateAdoptionDto dto, CancellationToken ct);
    Task DeleteAdoptionAsync(Guid id, CancellationToken ct);
    Task UpdateAdoptionStatusAsync(Guid id, UpdateAdoptionDto dto, CancellationToken ct);
}