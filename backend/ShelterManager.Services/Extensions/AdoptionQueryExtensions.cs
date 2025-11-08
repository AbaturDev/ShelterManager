using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Adoptions;

namespace ShelterManager.Services.Extensions;

public static class AdoptionQueryExtensions
{
    public static IQueryable<Adoption> ApplyFilters(this IQueryable<Adoption> query, AdoptionPageQueryFilter filter)
    {
        if (filter.Status?.Any() == true)
            query = query.Where(x => filter.Status.Contains(x.Status));
        
        if (filter.AnimalName is not null)
            query = query.Where(x => x.Animal.Name.Contains(filter.AnimalName));
        
        if (filter.AnimalIds?.Any() == true)
            query = query.Where(x => filter.AnimalIds.Contains(x.AnimalId));
        
        return query;
    }
}
