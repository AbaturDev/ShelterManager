namespace ShelterManager.Services.Dtos.Users;

public sealed record UserDetailsDto : UserDto
{
    public bool MustChangePassword { get; init; }
    public required string Role { get; init; }
}