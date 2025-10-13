namespace ShelterManager.Services.Dtos.DailyTask;

public sealed record DailyTaskEntryDto
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public DateTimeOffset? CompletedAt { get; init; }
    public Guid DailyTaskId { get; init; }
    public Guid? UserId { get; init; }
}