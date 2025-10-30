namespace ShelterManager.Services.Dtos.SystemInfo;

public sealed record StatisticsDto
{
    public required int AnimalsCount { get; init; }
    public required int CurrentAdoptionProcessCount { get; init; }
    public required int UpcomingEventsCount { get; init; }
}