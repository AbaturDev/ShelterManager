using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Breeds;

namespace ShelterManager.Services.Mappers;

public static class BreedMapper
{
    public static BreedDto MapToBreedDto(Breed breed)
    {
        var dto = new BreedDto
        {
            Name = breed.Name,
            SpeciesId = breed.SpeciesId
        };

        return dto;
    }
}