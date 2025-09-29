using ShelterManager.Services.Dtos.Accounts;
using LoginRequest = ShelterManager.Services.Dtos.Accounts.LoginRequest;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAccountService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
    Task ChangePasswordAsync(ChangePasswordRequest request);
}