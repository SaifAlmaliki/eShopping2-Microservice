namespace Shared.Pagination;

// The PaginationRequest record represents a request for paginated data
// It contains parameters for page index and page size with default values
public record PaginationRequest(int PageIndex = 0, int PageSize = 10);
