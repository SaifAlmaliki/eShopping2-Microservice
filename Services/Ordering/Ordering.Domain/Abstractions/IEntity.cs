// Purpose: Defines interfaces for entities, which are objects with unique identities.
// Entities have distinct identities that run through time and different states.

namespace Ordering.Domain.Abstractions;

// Interface for an entity with common properties.
public interface IEntity
{
    // Timestamp for when the entity was created.
    public DateTime? CreatedAt { get; set; }

    // User who created the entity.
    public string? CreatedBy { get; set; }

    // Timestamp for the last modification of the entity.
    public DateTime? LastModified { get; set; }

    // User who last modified the entity.
    public string? LastModifiedBy { get; set; }
}

// Generic interface for an entity with a specific type of identity.
// T is the type of the unique identifier for the entity (e.g., int, Guid, string).
public interface IEntity<T> : IEntity
{
    // Unique identifier for the entity.
    public T Id { get; set; }
}
