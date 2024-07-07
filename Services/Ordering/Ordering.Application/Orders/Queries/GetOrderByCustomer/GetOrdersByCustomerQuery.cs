namespace Ordering.Application.Orders.Queries.GetOrderByCustomer;

// The GetOrdersByCustomerQuery represents a query to retrieve orders by a given customer ID
// It takes a CustomerId (Guid) as input and implements the IQuery interface
public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;

// The GetOrdersByCustomerResult represents the result of a GetOrdersByCustomerQuery
// It contains an IEnumerable of OrderDto representing the retrieved orders
public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
