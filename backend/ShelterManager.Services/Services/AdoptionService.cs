using Microsoft.EntityFrameworkCore;
using ShelterManager.Core.Exceptions;
using ShelterManager.Core.Services.Abstractions;
using ShelterManager.Database.Contexts;
using ShelterManager.Database.Entities;
using ShelterManager.Database.Entities.Owned;
using ShelterManager.Database.Enums;
using ShelterManager.Services.Dtos.Adoptions;
using ShelterManager.Services.Dtos.Commons;
using ShelterManager.Services.Extensions;
using ShelterManager.Services.Services.Abstractions;

namespace ShelterManager.Services.Services;

public class AdoptionService : IAdoptionService
{
    private readonly ShelterManagerContext _context;
    private readonly TimeProvider _timeProvider;
    private readonly IUserContextService _userContext;
    
    public AdoptionService(ShelterManagerContext context, TimeProvider timeProvider, IUserContextService userContext)
    {
        _context = context;
        _timeProvider = timeProvider;
        _userContext = userContext;
    }
    
    public async Task<AdoptionDetailsDto> GetAdoptionAsync(Guid id, CancellationToken ct)
    {
        var adoption = await _context.Adoptions
            .Include(x => x.Animal)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (adoption is null)
        {
            throw new NotFoundException("Adoption not found");
        }

        return new AdoptionDetailsDto
        {
            Id = adoption.Id,
            CreatedAt = adoption.CreatedAt,
            UpdatedAt = adoption.UpdatedAt,
            Note = adoption.Note,
            Status = adoption.Status,
            StartAdoptionProcess = adoption.StartAdoptionProcess,
            AdoptionDate = adoption.AdoptionDate,
            AnimalName = adoption.Animal.Name,
            AnimalId = adoption.AnimalId,
            Person = new AdoptionPersonDto
            {
                Name = adoption.Person.Name,
                Surname = adoption.Person.Surname,
                DocumentId = adoption.Person.DocumentId,
                Pesel = adoption.Person.Pesel,
                Email = adoption.Person.Email,
                PhoneNumber = adoption.Person.PhoneNumber,
                City = adoption.Person.City,
                Street = adoption.Person.Street,
                PostalCode = adoption.Person.PostalCode
            }
        };
    }

    public async Task<PaginatedResponse<AdoptionDto>> ListAdoptionsAsync(AdoptionPageQueryFilter filter, CancellationToken ct)
    {
        var query = _context.Adoptions
            .Include(x => x.Animal)
            .AsNoTracking()
            .ApplyFilters(filter)
            .AsQueryable();
        
        var count = await query.CountAsync(ct);

        var items = await query
            .Select(x => new AdoptionDto
            {
                Id = x.Id,
                Status = x.Status,
                StartAdoptionProcess = x.StartAdoptionProcess,
                AdoptionDate = x.AdoptionDate,
                AnimalName = x.Animal.Name,
                AnimalId = x.AnimalId,
            })
            .Paginate(filter.Page, filter.PageSize)
            .ToListAsync(ct);

        return new PaginatedResponse<AdoptionDto>(items, filter.Page, filter.PageSize, count);
    }

    public async Task<Guid> CreateAdoptionAsync(CreateAdoptionDto dto, CancellationToken ct)
    {
        if (!await _context.Animals.AnyAsync(x => x.Id == dto.AnimalId, ct))
        {
            throw new BadRequestException("Can not create adoption for animal that not exists");
        }

        if (await _context.Adoptions
                .Where(x => x.Status == AdoptionStatus.Pending || x.Status == AdoptionStatus.Approved)
                .AnyAsync(x => x.AnimalId == dto.AnimalId, ct))
        {
            throw new BadRequestException("Cannot create adoption: this animal already has an ongoing or approved adoption.");
        }
            
        var adoption = new Adoption
        {
            Note = dto.Note,
            Person = new AdoptionPerson
            {
                Name = dto.Person.Name,
                Surname = dto.Person.Surname,
                Pesel = dto.Person.Pesel,
                DocumentId = dto.Person.DocumentId,
                Email = dto.Person.Email,
                PhoneNumber = dto.Person.PhoneNumber,
                City = dto.Person.City,
                Street = dto.Person.Street,
                PostalCode = dto.Person.PostalCode,
            },
            AnimalId = dto.AnimalId,
            StartAdoptionProcess = _timeProvider.GetUtcNow(),
            Status = AdoptionStatus.Pending
        };
        
        _context.Adoptions.Add(adoption);
        await _context.SaveChangesAsync(ct);
        
        return adoption.Id;
    }

    public async Task DeleteAdoptionAsync(Guid id, CancellationToken ct)
    {
        var adoption = await _context.Adoptions
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (adoption is null)
        {
            throw new NotFoundException("Adoption not found");
        }
        
        _context.Adoptions.Remove(adoption);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAdoptionStatusAsync(Guid id, UpdateAdoptionDto dto, CancellationToken ct)
    {
        var adoption = await _context.Adoptions
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (adoption is null)
        {
            throw new NotFoundException("Adoption not found");
        }

        if (adoption.Status != AdoptionStatus.Pending)
        {
            throw new BadRequestException("Only pending adoptions can be updated.");
        }
        
        adoption.Status = dto.Status;
        adoption.Note = dto.Note;

        if (dto.Status == AdoptionStatus.Approved)
        {
            if (dto.Event is not null)
            {
                adoption.AdoptionDate = dto.Event.PlannedAdoptionDate;

                _context.Events.Add(new Event
                {
                    AnimalId = adoption.AnimalId,
                    Date = dto.Event.PlannedAdoptionDate,
                    Title = dto.Event.Title,
                    Description = dto.Event.Description,
                    IsDone = false,
                    Location = dto.Event.Location,
                    UserId = _userContext.GetCurrentUserId()
                });
            }
            else
            {
                adoption.AdoptionDate = _timeProvider.GetUtcNow();
            }
        }
        
        await _context.SaveChangesAsync(ct);
    }
}