using ShelterManager.Database.Enums;

namespace ShelterManager.Services.Dtos.Animals;

public sealed record CreateAnimalDto
{
    public required string Name { get; init; }
    public required DateTimeOffset AdmissionDate { get; init; }
    public required Sex Sex { get; init; }
    public int? Age { get; init; }
    public string? Description { get; init; }
    public string? ImagePath { get; init; }
    public required Guid BreedId { get; init; }
}