﻿namespace Ordering.Domain.Abstractions;

// IDomainEvent class implements INotification to work with MediatR, a library for handling in-process messaging.
public interface IDomainEvent : INotification
{
    // Generate a new unique identifier for each event.
    Guid EventId => Guid.NewGuid();

    // Property to get the current UTC date and time when the event occurred.
    public DateTime OccuredOn => DateTime.UtcNow;

    // Public read-only property to get the fully qualified name of the event's type.
    // The fully qualified name includes the namespace, the class name, and the assembly information.
    // This is useful for identifying the exact type of the event in a unique manner.
    public string EventType => GetType().AssemblyQualifiedName;
}

/* 
* This class Defines a domain event, which is a notification about something that has happened in the domain.
* Domain events capture changes or significant occurrences within the domain.
*/

