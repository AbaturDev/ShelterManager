using ShelterManager.Database.Enums;

namespace ShelterManager.Services.Dtos.Adoptions;

public sealed record UpdateAdoptionDto
{
    public AdoptionStatus Status { get; init; }
    public string? Note { get; init; }
    public AdoptionEventDto? Event { get; set; }
}