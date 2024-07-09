namespace Shared.Messaging.Events;

/// <summary>
/// Represents the base class for all integration events.
/// Integration events are used to communicate between different microservices asynchronously.
/// </summary>
public record IntegrationEvent
{
    /// <summary>
    /// Gets a unique identifier for the event.
    /// Each time a new event is created, it is assigned a new GUID.
    /// </summary>
    public Guid Id => Guid.NewGuid();

    /// <summary>
    /// Gets the date and time when the event occurred.
    /// This is set to the current date and time when the event is created.
    /// </summary>
    public DateTime OccurredOn => DateTime.Now;

    /// <summary>
    /// Gets the fully qualified name of the event type.
    /// This helps in identifying the type of the event for handling purposes.
    /// </summary>
    public string EventType => GetType().AssemblyQualifiedName;
}
