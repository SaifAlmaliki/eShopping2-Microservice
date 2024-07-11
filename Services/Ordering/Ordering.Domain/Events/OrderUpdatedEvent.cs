namespace Ordering.Domain.Events;

// Define a record type called OrderUpdatedEvent, which is used to represent an event where an order is updated.
// The record takes an Order object as a parameter and implements the IDomainEvent interface.
public record OrderUpdatedEvent(Order order): IDomainEvent;
