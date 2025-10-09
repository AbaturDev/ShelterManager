using ShelterManager.Services.Dtos.Commons;

namespace ShelterManager.Services.Dtos.Events;

public sealed record EventDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required DateTimeOffset Date { get; init; }
    public bool IsDone { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public MoneyDto? Cost { get; set; }
    public required string Location { get; init; }
    public Guid AnimalId { get; init; }
    public Guid? UserId { get; init; }
    public Guid? CompletedByUserId { get; init; }
}