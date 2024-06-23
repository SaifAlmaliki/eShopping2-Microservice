// Purpose: Defines an abstract base class for entities, which are objects with a unique identity.
// Entities are objects that have distinct identities that run through time and different states.

namespace Ordering.Domain.Abstractions;

// Generic abstract class representing an entity.
// T is the type of the unique identifier for the entity (e.g., int, Guid, string).
public abstract class Entity<T> : IEntity<T>
{
    // Unique identifier for the entity.
    public T Id { get; set; }

    // Timestamp for when the entity was created.
    public DateTime? CreatedAt { get; set; }

    // User who created the entity.
    public string? CreatedBy { get; set; }

    // Timestamp for the last modification of the entity.
    public DateTime? LastModified { get; set; }

    // User who last modified the entity.
    public string? LastModifiedBy { get; set; }
}
