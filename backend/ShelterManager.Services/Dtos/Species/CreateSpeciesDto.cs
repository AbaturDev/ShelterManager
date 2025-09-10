namespace ShelterManager.Services.Dtos.Species;

public sealed record CreateSpeciesDto
{
    public required string Name { get; init; }
}