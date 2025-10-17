using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Mappers;

public static class AnimalMappers
{
    public static AnimalDto MapToAnimalDto(Animal animal)
    {
        var dto = new AnimalDto
        {
            Id = animal.Id,
            CreatedAt = animal.CreatedAt,
            UpdatedAt = animal.UpdatedAt,
            Name = animal.Name,
            Sex = animal.Sex,
            Description = animal.Description,
            AdmissionDate = animal.AdmissionDate,
            Age = animal.Age,
            Status = animal.Status,
            ImagePath = animal.ImagePath,
            Species = new AnimalSpeciesDto
            {
                Id = animal.Breed.SpeciesId,
                Name = animal.Breed.Species.Name,
                Breed = new AnimalBreedDto
                {
                    Id = animal.BreedId,
                    Name = animal.Breed.Name                    
                }
            }
        };
        
        return dto;
    }
}