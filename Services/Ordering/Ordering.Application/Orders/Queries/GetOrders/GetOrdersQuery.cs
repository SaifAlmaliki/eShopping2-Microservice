namespace Ordering.Application.Orders.Queries.GetOrders;

// The GetOrdersQuery represents a query to retrieve a paginated list of orders.
// It takes a PaginationRequest as input and implements the IQuery interface.
public record GetOrdersQuery(PaginationRequest PaginationRequest) : IQuery<GetOrdersResult>;

// The GetOrdersResult represents the result of a GetOrdersQuery.
// It contains a PaginatedResult of OrderDto representing the paginated list of orders.
public record GetOrdersResult(PaginatedResult<OrderDto> Orders);
