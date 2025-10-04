using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Dtos.Species;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Mappers;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class SpeciesService : ISpeciesService
{
    private readonly ShelterManagerContext _context;

    public SpeciesService(ShelterManagerContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedResponse<SpeciesDto>> ListSpeciesAsync(PageQueryFilter pageQueryFilter, CancellationToken ct)
    {
        var query = _context.Species
            .AsNoTracking()
            .AsQueryable();

        var count = await query.CountAsync(ct);

        var items = await query
            .Select(s => SpeciesMapper.MapToSpeciesDto(s))
            .Paginate(pageQueryFilter.Page, pageQueryFilter.PageSize)
            .ToListAsync(ct);

        return new PaginatedResponse<SpeciesDto>(items, pageQueryFilter.Page, pageQueryFilter.PageSize, count);
    }

    public async Task<SpeciesDto> GetSpeciesByIdAsync(Guid id, CancellationToken ct)
    {
        var species = await _context.Species
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (species is null)
        {
            throw new NotFoundException("Species not found");
        }

        var speciesDto = SpeciesMapper.MapToSpeciesDto(species);

        return speciesDto;
    }

    public async Task<Guid> CreateSpeciesAsync(CreateSpeciesDto dto, CancellationToken ct)
    {
        var speciesExists = await _context.Species
            .Where(s => s.Name == dto.Name)
            .AnyAsync(ct);

        if (speciesExists)
        {
            throw new BadRequestException("Species already exists");
        }
        
        var species = new Species
        {
            Name = dto.Name
        };

        _context.Species.Add(species);
        await _context.SaveChangesAsync(ct);

        return species.Id;
    }

    public async Task DeleteSpeciesAsync(Guid id, CancellationToken ct)
    {
        var species = await _context.Species
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (species is null)
        {
            throw new NotFoundException("Species not found");
        }

        _context.Species.Remove(species);
        await _context.SaveChangesAsync(ct);
    }
}