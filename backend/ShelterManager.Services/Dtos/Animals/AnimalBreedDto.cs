namespace ShelterManager.Services.Dtos.Animals;

public sealed record AnimalBreedDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}