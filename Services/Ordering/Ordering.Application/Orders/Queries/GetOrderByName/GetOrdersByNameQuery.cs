namespace Ordering.Application.Orders.Queries.GetOrderByName;

// The GetOrdersByNameQuery represents a query to retrieve orders by a given name
// It takes a Name (string) as input and implements the IQuery interface
public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;

// The GetOrdersByNameResult represents the result of a GetOrdersByNameQuery
// It contains an IEnumerable of OrderDto representing the retrieved orders
public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);
