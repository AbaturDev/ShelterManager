using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Species;

namespace ShelterManager.Services.Mappers;

public static class SpeciesMapper
{
    public static SpeciesDto MapToSpeciesDto(Species species)
    {
        var dto = new SpeciesDto
        {
            Name = species.Name
        };

        return dto;
    }
}