namespace ShelterManager.Services.Dtos.DailyTask;

public sealed record AddDailyTaskEntryDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}