using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Species;

namespace ShelterManager.Services.Mappers;

public static class SpeciesMappers
{
    public static SpeciesDto MapToSpeciesDto(Species species)
    {
        var dto = new SpeciesDto
        {
            Id = species.Id,
            CreatedAt = species.CreatedAt,
            UpdatedAt = species.UpdatedAt,
            Name = species.Name
        };

        return dto;
    }
}