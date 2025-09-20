namespace ShelterManager.Services.Dtos.Breeds;

public sealed record BreedDto
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
    public required string Name { get; init; }
    public required Guid SpeciesId { get; init; }
}