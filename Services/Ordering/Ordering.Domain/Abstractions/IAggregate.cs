// Purpose: Defines interfaces for aggregates, which are entities with unique identities and domain events.
// Aggregates help maintain consistency within their boundary by enforcing invariants.

namespace Ordering.Domain.Abstractions;

// Interface for an aggregate with domain events.
public interface IAggregate : IEntity
{
    // Read-only list of domain events associated with the aggregate.
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    // Method to clear the domain events and return the cleared events.
    IDomainEvent[] ClearDomainEvents();
}

// Generic interface for an aggregate with a specific type of identity.
// T is the type of the unique identifier for the aggregate (e.g., int, Guid, string).
public interface IAggregate<T> : IAggregate, IEntity<T>
{
}
