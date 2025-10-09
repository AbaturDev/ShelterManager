namespace ShelterManager.Services.Dtos.Users;

public record UserDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
}