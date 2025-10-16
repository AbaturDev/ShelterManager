using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Adoptions;

namespace ShelterManager.Services.Extensions;

public static class AdoptionQueryExtensions
{
    public static IQueryable<Adoption> ApplyFilters(this IQueryable<Adoption> query, AdoptionPageQueryFilter filter)
    {
        if (filter.Status is not null)
            query = query.Where(x => x.Status == filter.Status);
        
        if (filter.AnimalName is not null)
            query = query.Where(x => x.Animal.Name.Contains(filter.AnimalName));
        
        if (filter.AnimalsId is not null)
            query = query.Where(x => x.Animal.Id == filter.AnimalsId);
        
        return query;
    }
}
