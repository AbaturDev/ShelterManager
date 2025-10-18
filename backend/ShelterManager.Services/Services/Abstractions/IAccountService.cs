using ShelterManager.Services.Dtos.Accounts;
using LoginRequest = ShelterManager.Services.Dtos.Accounts.LoginRequest;

namespace ShelterManager.Services.Services.Abstractions;

public interface IAccountService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request, string languageCode);
    Task ChangePasswordAsync(ChangePasswordRequest request);
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task SendResetPasswordEmailAsync(ForgotPasswordRequest request, string lang);
    Task ResetPasswordAsync(ResetPasswordRequest request);
}