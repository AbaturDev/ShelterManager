namespace ShelterManager.Services.Dtos.Accounts;

public sealed record RegisterRequest
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public required string Role { get; init; }
}