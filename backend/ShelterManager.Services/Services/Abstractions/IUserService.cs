using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Users;

namespace ShelterManager.Services.Services.Abstractions;

public interface IUserService
{
    Task<PaginatedResponse<UserDto>> ListUsersAsync(PageQueryFilter queryFilter, CancellationToken ct);
    Task<UserDetailsDto> GetUserByIdAsync(Guid id, CancellationToken ct);
    Task<UserDetailsDto> GetCurrentUserAsync(CancellationToken ct);
    Task SoftDeleteUserAsync(Guid id, CancellationToken ct);
}