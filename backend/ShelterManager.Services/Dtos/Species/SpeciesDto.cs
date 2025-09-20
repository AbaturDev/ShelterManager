namespace ShelterManager.Services.Dtos.Species;

public sealed record SpeciesDto
{
    public required Guid Id { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }
    public required string Name { get; init; }
}