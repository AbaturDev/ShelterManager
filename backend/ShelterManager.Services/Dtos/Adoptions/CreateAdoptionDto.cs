namespace ShelterManager.Services.Dtos.Adoptions;

public sealed record CreateAdoptionDto
{
    public string? Note { get; set; }
    public required Guid AnimalId { get; set; }
    public required AdoptionPersonDto Person { get; set; }
}