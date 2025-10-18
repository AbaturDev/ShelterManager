namespace ShelterManager.Services.Dtos.Accounts;

public sealed record ForgotPasswordRequest
{
    public required string Email { get; init; }
}