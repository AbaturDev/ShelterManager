using ShelterManager.Database.Enums;

namespace ShelterManager.Services.Dtos.Animals;

public sealed record UpdateAnimalDto
{
    public required string Name { get; init; }
    public required DateTimeOffset AdmissionDate { get; init; }
    public int? Age { get; init; }
    public string? Description { get; init; }
    public required AnimalStatus Status { get; init; }
}