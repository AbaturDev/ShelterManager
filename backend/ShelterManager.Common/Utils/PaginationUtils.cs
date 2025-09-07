namespace ShelterManager.Common.Utils;

public static class PaginationUtils
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page, int pageSize)
    {
        return source
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
    
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        return source
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }
}