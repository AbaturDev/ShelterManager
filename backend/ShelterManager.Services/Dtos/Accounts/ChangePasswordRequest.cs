namespace ShelterManager.Services.Dtos.Accounts;

public sealed record ChangePasswordRequest
{
    public required string Email { get; init; }
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}