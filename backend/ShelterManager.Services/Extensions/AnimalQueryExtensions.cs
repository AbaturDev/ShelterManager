using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Animals;

namespace ShelterManager.Services.Extensions;

public static class AnimalQueryExtensions
{
    public static IQueryable<Animal> ApplyFilters(this IQueryable<Animal> query, AnimalPageQueryFilter filter)
    {
        if (filter.Status is not null)
            query = query.Where(x => x.Status == filter.Status);

        if (filter.Sex is not null)
            query = query.Where(x => x.Sex == filter.Sex);

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(x => x.Name.Contains(filter.Name));
        
        return query;
    }
}