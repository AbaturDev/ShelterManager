using ShelterManager.Database.Enums;

namespace ShelterManager.Services.Dtos.Adoptions;

public record AdoptionDto
{
    public Guid Id { get; init; }
    public AdoptionStatus Status { get; init; }
    public required string AnimalName { get; init; }
    public Guid AnimalId { get; init; }
    public DateTimeOffset StartAdoptionProcess { get; init; }
    public DateTimeOffset? AdoptionDate { get; init; }
}