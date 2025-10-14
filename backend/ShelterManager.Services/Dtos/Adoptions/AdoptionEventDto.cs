namespace ShelterManager.Services.Dtos.Adoptions;

public sealed record AdoptionEventDto
{
    public required DateTimeOffset PlannedAdoptionDate { get; init; }
    public required string Title { get; init; }
    public required string Location { get; init; }
    public string? Description { get; init; }

}