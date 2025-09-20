using Microsoft.EntityFrameworkCore;
using ShelterManager.Common.Dtos;
using ShelterManager.Common.Exceptions;
using ShelterManager.Common.Utils;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Animals;
using ShelterManager.Services.Mappers;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AnimalService : IAnimalService
{
    private readonly ShelterManagerContext _context;

    public AnimalService(ShelterManagerContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedResponse<AnimalDto>> ListAnimalsAsync(PageQueryFilter pageQueryFilter, CancellationToken ct)
    {
        var query = _context.Animals.AsNoTracking();

        var count = await query.CountAsync(ct);

        var animals = await query
            .Select(a => AnimalMapper.MapToAnimalDto(a))
            .Paginate(pageQueryFilter.Page, pageQueryFilter.PageSize)
            .ToListAsync(ct);

        return new PaginatedResponse<AnimalDto>(animals, pageQueryFilter.Page, pageQueryFilter.PageSize, count);
    }

    public async Task<AnimalDto> GetAnimalByIdAsync(Guid id, CancellationToken ct)
    {
        var animal = await _context.Animals
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (animal is null)
        {
            throw new NotFoundException("Animal not found");
        }

        var dto = AnimalMapper.MapToAnimalDto(animal);

        return dto;
    }

    public async Task<Guid> CreateAnimalAsync(CreateAnimalDto animalDto, CancellationToken ct)
    {
        var breedExist = await _context.Breeds
            .Where(b => b.Id == animalDto.BreedId)
            .AnyAsync(ct);

        if (!breedExist)
        {
            throw new BadRequestException("Can not create animal for breed that not exists");
        }

        var animal = new Animal
        {
            Name = animalDto.Name,
            AdmissionDate = animalDto.AdmissionDate,
            Age = animalDto.Age,
            Status = AnimalStatus.InShelter,
            Description = animalDto.Description,
            ImagePath = animalDto.ImagePath,
            BreedId = animalDto.BreedId
        };

        _context.Animals.Add(animal);
        await _context.SaveChangesAsync(ct);

        return animal.Id;
    }

    public async Task DeleteAnimalAsync(Guid id, CancellationToken ct)
    {
        var animal = await _context.Animals
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (animal is null)
        {
            throw new NotFoundException("Animal not found");
        }

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAnimalAsync(Guid id, UpdateAnimalDto dto, CancellationToken ct)
    {
        var animal = await _context.Animals
            .FirstOrDefaultAsync(a => a.Id == id, ct);

        if (animal is null)
        {
            throw new NotFoundException("Animal not found");
        }

        animal.Name = dto.Name;
        animal.AdmissionDate = dto.AdmissionDate;
        animal.Age = dto.Age;
        animal.Description = dto.Description;
        animal.ImagePath = dto.ImagePath;
        animal.Status = dto.Status;
        
        await _context.SaveChangesAsync(ct);
    }
}