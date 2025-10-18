using ShelterManager.Database.Enums;

namespace ShelterManager.Services.Dtos.Animals;

public sealed record AnimalDto
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
    public required Sex Sex { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset AdmissionDate { get; init; }
    public required AnimalStatus Status { get; init; }
    public int? Age { get; init; }
    public string? ImagePath { get; init; }
    public string? Description { get; init; }
    public required AnimalSpeciesDto Species { get; init; }
}