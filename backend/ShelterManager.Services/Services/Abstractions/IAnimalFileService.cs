using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAnimalFileService
{
    Task UploadAnimalProfileImageAsync(Guid id, string fileName, Stream imageStream, CancellationToken ct);
    Task<FileStreamDto> GetAnimalProfileImageAsync(Guid id, CancellationToken ct);
    Task UploadAnimalFileAsync(Guid id, string fileName, Stream fileStream, CancellationToken ct);
    Task<PaginatedResponse<FileDto>> ListAnimalFilesAsync(Guid id, PageQueryFilter queryFilter, CancellationToken ct);
    Task<FileStreamDto> GetAnimalFileAsync(Guid id, string fileName, CancellationToken ct);
    Task DeleteAnimalFileAsync(Guid id, string fileName, CancellationToken ct);
}