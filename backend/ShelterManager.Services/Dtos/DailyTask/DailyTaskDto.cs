namespace ShelterManager.Services.Dtos.DailyTask;

public sealed record DailyTaskDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public DateOnly Date { get; init; }
    public Guid AnimalId { get; init; }
    public List<DailyTaskEntryDto> Entries { get; init; }
}