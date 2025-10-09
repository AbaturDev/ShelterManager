namespace ShelterManager.Services.Dtos.Accounts;

public sealed record RefreshTokenRequest
{
    public required string RefreshToken { get; init; }
}