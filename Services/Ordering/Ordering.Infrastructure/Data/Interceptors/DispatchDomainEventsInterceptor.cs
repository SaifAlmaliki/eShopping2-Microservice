namespace Ordering.Infrastructure.Data.Interceptors;

// Interceptor class to dispatch domain events before saving changes to the database
public class DispatchDomainEventsInterceptor(IMediator _mediator) : SaveChangesInterceptor
{

    // Override for synchronous saving changes
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Dispatch domain events before saving changes
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result); // Call the base method to continue saving
    }

    // Override for asynchronous saving changes
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving changes asynchronously
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken); // Call the base method to continue saving
    }

    // Method to dispatch domain events
    public async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return; // If context is null, return immediately

        // Get all aggregates that have domain events
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>() // Track changes in entities that implement IAggregate
            .Where(a => a.Entity.DomainEvents.Any()) // Filter aggregates that have domain events
            .Select(a => a.Entity); // Select the aggregate entities

        // Get all domain events from the aggregates
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents) // Select all domain events from each aggregate
            .ToList(); // Convert to list

        // Clear domain events from the aggregates after capturing them
        aggregates.ToList().ForEach(a => a.ClearDomainEvents());

        // Publish each domain event using the mediator
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
