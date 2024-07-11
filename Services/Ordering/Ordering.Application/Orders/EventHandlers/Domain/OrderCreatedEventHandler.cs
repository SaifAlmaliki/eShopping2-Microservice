namespace Ordering.Application.Orders.EventHandlers.Domain;

// The OrderCreatedEventHandler handles the OrderCreatedEvent from Ordering.Domain
public class OrderCreatedEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        // Log information that the domain event has been handled
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        // Check if the "OrderFulfillment" feature is enabled
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            // Convert the order from the domain event to an OrderDto
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();

            // Publish the integration event to the message bus
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
