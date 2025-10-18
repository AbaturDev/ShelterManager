namespace ShelterManager.Services.Dtos.Animals;

public sealed record AnimalSpeciesDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required AnimalBreedDto Breed { get; init; }
}