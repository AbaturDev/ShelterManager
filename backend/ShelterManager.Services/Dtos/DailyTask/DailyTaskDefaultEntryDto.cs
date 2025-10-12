namespace ShelterManager.Services.Dtos.DailyTask;

public sealed record DailyTaskDefaultEntryDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public Guid AnimalId { get; init; }
}