namespace Ordering.Application.Orders.Queries.GetOrderByName;

// The GetOrdersByNameQueryHandler handles the GetOrdersByNameQuery
public class GetOrdersByNameQueryHandler(IApplicationDbContext _dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        // Retrieve orders from the database context using the provided name
        var orders = await _dbContext.Orders
            .Include(o => o.OrderItems) // Include related OrderItems in the query
            .AsNoTracking()             // Perform the query without tracking changes (read-only)
            .Where(o => o.OrderName.Value.Contains(query.Name)) // Filter orders by name
            .OrderBy(o => o.OrderName.Value) // Order the results by order name
            .ToListAsync(cancellationToken); // Execute the query and return the results as a list

        // Convert the list of Order entities to a list of OrderDto and return the result
        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }
}
