namespace Ordering.Application.Orders.Queries.GetOrders;

// The GetOrdersQueryHandler handles the GetOrdersQuery
public class GetOrdersQueryHandler(IApplicationDbContext _dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        // Extract pagination parameters from the query
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        // Get the total count of orders in the database
        // LongCountAsync performs an asynchronous count operation and returns a long integer, which is useful for large datasets
        var totalCount = await _dbContext.Orders.LongCountAsync(cancellationToken);

        // Retrieve orders with pagination, including related OrderItems
        var orders = await _dbContext.Orders
                       .Include(o => o.OrderItems)      // Include related OrderItems in the query
                       .OrderBy(o => o.OrderName.Value) // Order the results by order name
                       .Skip(pageSize * pageIndex)  // Skip the records for previous pages
                       .Take(pageSize)              // Take only the records for the current page
                       .ToListAsync(cancellationToken); // Execute the query and return the results as a list

        // Convert the list of Order entities to a list of OrderDto and return the paginated result
        return new GetOrdersResult(
            new PaginatedResult<OrderDto>(
                pageIndex,
                pageSize,
                totalCount,
                orders.ToOrderDtoList())); // Use ToOrderDtoList extension method to convert entities to DTOs
    }
}