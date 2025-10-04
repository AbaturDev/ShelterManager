using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Dtos.Events;

public sealed record CreateEventDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public MoneyDto? Cost { get; init; }
    public required string Location { get; init; }
    public Guid AnimalId { get; init; }
    public Guid UserId { get; init; }
}