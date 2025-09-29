namespace ShelterManager.Services.Dtos.Accounts;

public sealed record LoginResponse
{
    public required string JwtToken { get; init; }
}