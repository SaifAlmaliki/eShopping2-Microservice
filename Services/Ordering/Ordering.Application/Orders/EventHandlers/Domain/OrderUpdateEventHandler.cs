namespace Ordering.Application.Orders.EventHandlers.Domain;

// The OrderUpdatedEventHandler handles the OrderUpdatedEvent
public class OrderUpdatedEventHandler
    (ILogger<OrderUpdatedEventHandler> _logger) : INotificationHandler<OrderUpdatedEvent>
{
    // Handles the OrderUpdatedEvent
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        // Log information that the domain event has been handled
        _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

        // Return a completed task as no further processing is needed
        return Task.CompletedTask;
    }
}
