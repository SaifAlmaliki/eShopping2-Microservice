namespace Ordering.Application.Orders.EventHandlers.Domain;

// The OrderCreatedEventHandler handles the OrderCreatedEvent
public class OrderCreatedEventHandler
    (IPublishEndpoint _publishEndpoint, IFeatureManager _featureManager, ILogger<OrderCreatedEventHandler> _logger)
    : INotificationHandler<OrderCreatedEvent>
{
    // Handles the OrderCreatedEvent
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Log information that the domain event has been handled
        _logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        // Check if the "OrderFulfillment" feature is enabled
        if (await _featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            // Convert the order from the domain event to an OrderDto
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

            // Publish the integration event to the message bus
            await _publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
