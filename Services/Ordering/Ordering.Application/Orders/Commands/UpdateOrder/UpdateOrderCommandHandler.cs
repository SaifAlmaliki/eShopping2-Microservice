namespace Ordering.Application.Orders.Commands.UpdateOrder;

// The UpdateOrderCommandHandler handles the UpdateOrderCommand
public class UpdateOrderCommandHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        // Extract the Order ID from the command
        var orderId = OrderId.Of(command.Order.Id);

        // Retrieve the existing order from the database using the Order ID
        var order = await dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        // If the order is not found, throw an OrderNotFoundException
        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        // Update the existing order with new values from the command
        UpdateOrderWithNewValues(order, command.Order);

        // Mark the order as updated in the database context
        dbContext.Orders.Update(order);

        // Save changes to the database
        await dbContext.SaveChangesAsync(cancellationToken);

        // Return the result indicating the update was successful
        return new UpdateOrderResult(true);
    }

    // Updates the existing Order entity with new values from the OrderDto
    public void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        // Create updated ShippingAddress entity from the OrderDto's ShippingAddress
        var updatedShippingAddress = Address.Of(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode
        );

        // Create updated BillingAddress entity from the OrderDto's BillingAddress
        var updatedBillingAddress = Address.Of(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode
        );

        // Create updated Payment entity from the OrderDto's Payment details
        var updatedPayment = Payment.Of(
            orderDto.Payment.CardName,
            orderDto.Payment.CardNumber,
            orderDto.Payment.Expiration,
            orderDto.Payment.Cvv,
            orderDto.Payment.PaymentMethod
        );

        // Update the order with the new values
        order.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: updatedShippingAddress,
            billingAddress: updatedBillingAddress,
            payment: updatedPayment,
            status: orderDto.Status
        );
    }
}
