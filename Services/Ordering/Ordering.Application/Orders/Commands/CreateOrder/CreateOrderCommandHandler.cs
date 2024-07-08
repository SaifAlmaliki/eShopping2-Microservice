namespace Ordering.Application.Orders.Commands.CreateOrder;

// The CreateOrderCommandHandler handles the CreateOrderCommand
public class CreateOrderCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Create a new Order entity from the command object
        var order = CreateNewOrder(command.Order);

        // Add the new order to the database context
        dbContext.Orders.Add(order);

        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return the result containing the new order's ID
        return new CreateOrderResult(order.Id.Value);
    }

    // Creates a new Order entity from the OrderDto
    private Order CreateNewOrder(OrderDto orderDto)
    {
        // Create a ShippingAddress entity from the OrderDto's ShippingAddress
        var shippingAddress = Address.Of(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode
        );

        // Create a BillingAddress entity from the OrderDto's BillingAddress
        var billingAddress = Address.Of(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode
        );

        // Create a new Order entity using factory method and the provided details
        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod
            )
        );

        // Add each OrderItemDto to the new order
        foreach (var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(
                ProductId.Of(orderItemDto.ProductId),
                orderItemDto.Quantity,
                orderItemDto.Price
            );
        }

        // Return the newly created order
        return newOrder;
    }
}
