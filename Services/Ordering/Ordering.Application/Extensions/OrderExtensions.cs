namespace Ordering.Application.Extensions;

// The OrderExtensions class provides extension methods for converting Order entities to OrderDto objects
public static class OrderExtensions
{
    // ToOrderDtoList: Converts an IEnumerable of Order entities to an IEnumerable of OrderDto objects
    public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
    {
        // Use LINQ to project each Order entity to an OrderDto object
        return orders.Select(order => new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress!,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress!,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto(
                order.Payment.CardName!,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod
            ),
            Status: order.Status,
            OrderItems: order.OrderItems.Select(oi => new OrderItemDto(
                oi.OrderId.Value,
                oi.ProductId.Value,
                oi.Quantity,
                oi.Price
            )).ToList()
        ));
    }

    // ToOrderDto: Converts a single Order entity to an OrderDto object
    public static OrderDto ToOrderDto(this Order order)
    {
        return CreateOrderDto(order);
    }

    // Helper method to create an OrderDto from an Order entity
    private static OrderDto CreateOrderDto(Order order)
    {
        return new OrderDto(
            Id: order.Id.Value,
            CustomerId: order.CustomerId.Value,
            OrderName: order.OrderName.Value,
            ShippingAddress: new AddressDto(
                order.ShippingAddress.FirstName,
                order.ShippingAddress.LastName,
                order.ShippingAddress.EmailAddress!,
                order.ShippingAddress.AddressLine,
                order.ShippingAddress.Country,
                order.ShippingAddress.State,
                order.ShippingAddress.ZipCode
            ),
            BillingAddress: new AddressDto(
                order.BillingAddress.FirstName,
                order.BillingAddress.LastName,
                order.BillingAddress.EmailAddress!,
                order.BillingAddress.AddressLine,
                order.BillingAddress.Country,
                order.BillingAddress.State,
                order.BillingAddress.ZipCode
            ),
            Payment: new PaymentDto(
                order.Payment.CardName!,
                order.Payment.CardNumber,
                order.Payment.Expiration,
                order.Payment.CVV,
                order.Payment.PaymentMethod
            ),
            Status: order.Status,
            OrderItems: order.OrderItems.Select(oi => new OrderItemDto(
                oi.OrderId.Value,
                oi.ProductId.Value,
                oi.Quantity,
                oi.Price
            )).ToList()
        );
    }
}

/*
 * These extension methods facilitate the transformation of domain entities to data transfer objects (DTOs), 
 * which are used to transfer data between layers in the application.
 * The OrderExtensions class provides extension methods to convert Order entities => OrderDto objects.
 * The ToOrderDtoList method: converts an IEnumerable of Order entities to an IEnumerable of OrderDto objects,
 * using LINQ to project each Order entity to an OrderDto object. 
 * The ToOrderDto method: converts a single Order entity to an OrderDto object, using the helper method CreateOrderDto  to perform the conversion.
 */
