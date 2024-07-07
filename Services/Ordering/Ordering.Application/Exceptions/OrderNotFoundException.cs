namespace Ordering.Application.Exceptions;

// OrderNotFoundException is a custom exception that inherits from NotFoundException
public class OrderNotFoundException : NotFoundException
{
    // The constructor calls the base constructor of the NotFoundException class, passing the entity name "Order" and the provided id
    public OrderNotFoundException(Guid id) : base("Order", id)
    {
        // The base constructor of NotFoundException is called with the entity name "Order" and the provided Order ID
    }
}
