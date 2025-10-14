namespace ShelterManager.Services.Dtos.Adoptions;

public sealed record AdoptionDetailsDto : AdoptionDto
{
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public required AdoptionPersonDto Person { get; init; }
    public string? Note { get; init; }
}