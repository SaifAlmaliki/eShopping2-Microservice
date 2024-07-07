namespace Ordering.Application.Dtos;

// The OrderDto represents the details of an order.
// This DTO is crucial for transferring complete order information within the application.
public record OrderDto(
   Guid Id,
   Guid CustomerId,
   string OrderName,
   AddressDto ShippingAddress,
   AddressDto BillingAddress,
   PaymentDto Payment,
   OrderStatus Status,
   List<OrderItemDto> OrderItems);
