namespace Ordering.Domain.Events;

// Define a record type called OrderCreatedEvent, which is used to represent an event where a new order is created.
// The record takes an Order object as a parameter and implements the IDomainEvent interface.
public record OrderCreatedEvent(Order order) : IDomainEvent;
