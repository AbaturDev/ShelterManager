namespace ShelterManager.Services.Dtos.Commons;

public sealed record PaginatedResponse<T>
{
    public ICollection<T> Items { get; set; } = null!;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    
    public PaginatedResponse(ICollection<T> items, int page, int pageSize, int totalItemsCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
    }
}