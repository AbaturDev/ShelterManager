namespace ShelterManager.Services.Dtos.Animals;

public sealed record CreateAnimalDto
{
    public required string Name { get; set; }
}