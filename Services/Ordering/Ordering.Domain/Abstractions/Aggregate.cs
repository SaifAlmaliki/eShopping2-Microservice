namespace Ordering.Domain.Abstractions;

// Generic abstract class representing an aggregate root.
// TId is the type of the unique identifier for the aggregate (e.g., int, Guid, string).
public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    // List to store domain events associated with the aggregate.
    // Domain events are things that have happened in the past that you want other parts of the system to be aware of.
    private readonly List<IDomainEvent> _domainEvents;

    // Provides read-only access to the domain events.
    // This ensures that the domain events can be read but not modified directly from outside the class.
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    // Method to add a domain event to the list.
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    // Method to clear the domain events and return the cleared events.
    // This is useful for processing the events and then removing them from the aggregate to avoid reprocessing.
    public IDomainEvent[] ClearDomainEvents()
    {
        // Convert the list of domain events to an array.
        IDomainEvent[] dequeuedEvents = _domainEvents.ToArray();
        // Clear the list of domain events.
        _domainEvents.Clear();
        // Return the array of cleared events.
        return dequeuedEvents;
    }
}


// Purpose: Defines an abstract base class for aggregates, which are entities with unique identities and domain events.
// Aggregates represent a cluster of domain objects that can be treated as a single unit. 
// They help maintain consistency within the boundary by enforcing invariants.

