namespace ShelterManager.Services.Dtos.DailyTask;

public sealed record UpdateDefaultDailyTaskEntryDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
}