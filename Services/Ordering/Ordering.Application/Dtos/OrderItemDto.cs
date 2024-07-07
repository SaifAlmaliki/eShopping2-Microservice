namespace Ordering.Application.Dtos;

// The OrderItemDto represents an item within an order.
// This DTO is essential for capturing details of each item in an order.
public record OrderItemDto(
    Guid OrderId,
    Guid ProductId,
    int Quantity,
    decimal Price);
