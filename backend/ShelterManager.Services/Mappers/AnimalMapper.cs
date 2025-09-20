using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Mappers;

public static class AnimalMapper
{
    public static AnimalDto MapToAnimalDto(Animal animal)
    {
        var dto = new AnimalDto
        {
            Id = animal.Id,
            CreatedAt = animal.CreatedAt,
            UpdatedAt = animal.UpdatedAt,
            Name = animal.Name,
            Description = animal.Description,
            AdmissionDate = animal.AdmissionDate,
            Age = animal.Age,
            Status = animal.Status,
            ImagePath = animal.ImagePath
        };
        
        return dto;
    }
}