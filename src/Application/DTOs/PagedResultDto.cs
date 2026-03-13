namespace UserApication.DTOs;

/// <summary>Standardized paginated response wrapper.</summary>
public sealed record PagedResultDto<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
)
{
    public static PagedResultDto<T> Create(IEnumerable<T> items, int totalCount, int page, int pageSize)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new PagedResultDto<T>(items, totalCount, page, pageSize, totalPages);
    }
}
