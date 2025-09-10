namespace ShelterManager.Services.Dtos.Breeds;

public sealed record BreedDto
{
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
}