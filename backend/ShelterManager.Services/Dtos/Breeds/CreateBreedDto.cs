namespace ShelterManager.Services.Dtos.Breeds;

public record CreateBreedDto
{
    public required string Name { get; init; }
}