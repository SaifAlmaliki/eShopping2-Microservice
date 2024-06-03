// This file defines the PaginatedResult class, which is used to encapsulate
// paginated results for any entity type. It includes information about the
// current page, page size, total count of items, and the data for the current page.

namespace Blazor.UI.Models.Ordering;

// Represents a paginated result set for a specific entity type
// TEntity is a generic type parameter constrained to classes
public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    // The current page index
    public int PageIndex { get; } = pageIndex;

    // The size of each page
    public int PageSize { get; } = pageSize;

    // The total count of items across all pages
    public long Count { get; } = count;

    // The data for the current page
    // This property holds the subset of items that belong to the current page.
    // It is an enumerable collection of TEntity, allowing for iteration over the items.
    // This is useful in scenarios where the data is displayed in a paginated format
    public IEnumerable<TEntity> Data { get; } = data;
}
