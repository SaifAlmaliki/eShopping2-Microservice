namespace Ordering.Application.Orders.Queries.GetOrderByCustomer;

// The GetOrdersByCustomerQueryHandler handles the GetOrdersByCustomerQuery
public class GetOrdersByCustomerQueryHandler(IApplicationDbContext _dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        // Retrieve orders from the database context using the provided customer ID
        var orders = await _dbContext.Orders
                        .Include(o => o.OrderItems) // Include related OrderItems in the query
                        .AsNoTracking()             // Perform the query without tracking changes (read-only)
                        .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId)) // Filter orders by customer ID
                        .OrderBy(o => o.OrderName.Value) // Order the results by order name
                        .ToListAsync(cancellationToken); // Execute the query and return the results as a list

        // Convert the list of Order entities to a list of OrderDto and return the result
        return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
    }
}
