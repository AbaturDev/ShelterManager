namespace ShelterManager.Services.Dtos.Accounts;

public sealed record ResetPasswordRequest
{
    public required string Email { get; init; }
    public required string Token { get; init; }
    public required string NewPassword { get; init; }
}