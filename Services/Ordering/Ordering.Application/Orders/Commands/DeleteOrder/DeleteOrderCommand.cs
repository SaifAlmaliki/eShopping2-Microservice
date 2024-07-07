namespace Ordering.Application.Orders.Commands.DeleteOrder;

// The DeleteOrderCommand represents a command to delete an existing order
// It takes an OrderId (Guid) as input and implements the ICommand interface
public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult>;

// The DeleteOrderResult represents the result of a DeleteOrderCommand
// It contains a boolean indicating whether the deletion was successful
public record DeleteOrderResult(bool IsSuccess);

// The DeleteOrderCommandValidator validates the DeleteOrderCommand
// It ensures that the OrderId is not empty
public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        // Ensure the OrderId property is not empty
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
    }
}
