namespace Ordering.Application.Orders.Commands.CreateOrder;

// The CreateOrderCommand represents a command to create a new order
// It takes an OrderDto as input and implements the ICommand interface
public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

// The CreateOrderResult represents the result of a CreateOrderCommand
// It contains the ID of the created order
public record CreateOrderResult(Guid Id);

// The CreateOrderCommandValidator validates the CreateOrderCommand
// It ensures that the OrderName is not empty, the CustomerId is not null, and the OrderItems collection is not empty
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        // Ensure the OrderName property of the OrderDto is not empty
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage("Order name is required");

        // Ensure the CustomerId property of the OrderDto is not null
        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage("Customer ID is required");

        // Ensure the OrderItems collection of the OrderDto is not empty
        RuleFor(x => x.Order.OrderItems)
            .NotEmpty()
            .WithMessage("Order items should not be empty");
    }
}
