namespace Shared.Pagination;

// The PaginatedResult class provides a structure for paginated results
// TEntity represents the type of the data being paginated
public class PaginatedResult<TEntity>
    (int _pageIndex, int _pageSize, long _count, IEnumerable<TEntity> _data)
    where TEntity : class
{
    public int PageIndex { get; } = _pageIndex;         // The current page index (0-based)
    public int PageSize { get; } = _pageSize;           // The size of each page
    public long Count { get; } = _count;                // The total count of items available
    public IEnumerable<TEntity> Data { get; } = _data;  // The data items for the current page
}
