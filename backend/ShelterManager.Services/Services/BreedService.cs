using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Breeds;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Mappers;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class BreedService : IBreedService
{
    private readonly ShelterManagerContext _context;
    
    public BreedService(ShelterManagerContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedResponse<BreedDto>> ListBreedsAsync(PageQueryFilter pageQueryFilter, Guid speciesId, CancellationToken ct)
    {
        var query = _context.Breeds
            .AsNoTracking()
            .Where(b => b.SpeciesId == speciesId)
            .AsQueryable();

        var count = await query.CountAsync(ct);

        var items = await query
            .Select(b => BreedMappers.MapToBreedDto(b))
            .Paginate(pageQueryFilter.Page, pageQueryFilter.PageSize)
            .ToListAsync(ct);

        return new PaginatedResponse<BreedDto>(items, pageQueryFilter.Page, pageQueryFilter.PageSize, count);
    }

    public async Task<BreedDto> GetBreedByIdAsync(Guid id, Guid speciesId, CancellationToken ct)
    {
        var breed = await _context.Breeds
            .AsNoTracking()
            .Where(b => b.SpeciesId == speciesId)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        if (breed is null)
        {
            throw new NotFoundException("Breed not found");
        }

        var dto = BreedMappers.MapToBreedDto(breed);

        return dto;
    }

    public async Task<Guid> CreateBreedAsync(CreateBreedDto dto, Guid speciesId, CancellationToken ct)
    {
        var species = await _context.Species
            .Include(s => s.Breeds)
            .FirstOrDefaultAsync(s => s.Id == speciesId, ct);

        if (species is null)
        {
            throw new BadRequestException("Can not create breed for species that not exists");
        }

        var breedExists = species.Breeds
            .Any(b => b.Name == dto.Name);

        if (breedExists)
        {
            throw new BadRequestException("Breed already exists for this species");
        }

        var breed = new Breed
        {
            Name = dto.Name,
        };

        species.Breeds.Add(breed);

        await _context.SaveChangesAsync(ct);

        return breed.Id;
    }

    public async Task DeleteBreedAsync(Guid id, Guid speciesId, CancellationToken ct)
    {
        var breed = await _context.Breeds
            .Where(b => b.SpeciesId == speciesId)
            .FirstOrDefaultAsync(b => b.Id == id, ct);

        if (breed is null)
        {
            throw new NotFoundException("Breed not found");
        }

        _context.Breeds.Remove(breed);
        await _context.SaveChangesAsync(ct);
    }
}