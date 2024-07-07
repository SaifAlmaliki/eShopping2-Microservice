namespace Ordering.Application.Orders.EventHandlers.Integration;

// The BasketCheckoutEventHandler handles the BasketCheckoutEvent
public class BasketCheckoutEventHandler
    (ISender _sender, ILogger<BasketCheckoutEventHandler> _logger) : IConsumer<BasketCheckoutEvent>
{
    // Handles the BasketCheckoutEvent
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        // Log information that the integration event has been handled
        _logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        // Map the incoming event data to a CreateOrderCommand
        var command = MapToCreateOrderCommand(context.Message);

        // Send the command to initiate the order creation process
        await _sender.Send(command);
    }

    // Maps the BasketCheckoutEvent data to a CreateOrderCommand
    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        // Create an AddressDto from the incoming event data
        var addressDto = new AddressDto(
            message.FirstName,
            message.LastName,
            message.EmailAddress,
            message.AddressLine,
            message.Country,
            message.State,
            message.ZipCode
        );

        // Create a PaymentDto from the incoming event data
        var paymentDto = new PaymentDto(
            message.CardName,
            message.CardNumber,
            message.Expiration,
            message.CVV,
            message.PaymentMethod
        );

        // Generate a new Order ID
        var orderId = Guid.NewGuid();

        // Create an OrderDto with the mapped data and a set of hard-coded OrderItemDto
        var orderDto = new OrderDto(
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: OrderStatus.Pending,
            OrderItems: new List<OrderItemDto>
            {
                new OrderItemDto(orderId, new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"), 2, 500),
                new OrderItemDto(orderId, new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"), 1, 400)
            }
        );

        // Return the CreateOrderCommand with the created OrderDto
        return new CreateOrderCommand(orderDto);
    }
}

/*
 * The BasketCheckoutEventHandler class handles the BasketCheckoutEvent by consuming the event, 
 * logging that the event was handled, mapping the event data to a CreateOrderCommand, and 
 * sending the command to create a new order. It uses the MapToCreateOrderCommand method to 
 * convert the event data into an OrderDto, which includes the customer's address, payment details, 
 * and order items. This ensures that the order fulfillment process is initiated based on the 
 * basket checkout event.
 */
