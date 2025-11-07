using ShelterManager.Database.Entities;
using ShelterManager.Services.Dtos.Events;

namespace ShelterManager.Services.Extensions;

public static class EventQueryExtensions
{
    public static IQueryable<Event> ApplyFilters(this IQueryable<Event> query, EventPageQueryFilter filter)
    {
        if (filter.AnimalIds?.Any() == true)
            query = query.Where(x => filter.AnimalIds.Contains(x.AnimalId));
        
        if (filter.IsDone.HasValue)
            query = query.Where(x => x.IsDone == filter.IsDone);
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(x => x.Title.Contains(filter.Title));

        return query;
    }
}