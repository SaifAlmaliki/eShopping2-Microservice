namespace Ordering.Application.Orders.Commands.UpdateOrder;

// The UpdateOrderCommand represents a command to update an existing order
// It takes an OrderDto as input and implements the ICommand interface
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

// The UpdateOrderResult represents the result of an UpdateOrderCommand
// It contains a boolean indicating whether the update was successful
public record UpdateOrderResult(bool IsSuccess);

// The UpdateOrderCommandValidator validates the UpdateOrderCommand
// It ensures that the OrderDto contains the necessary information for an update
public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        // Ensure the Id property of the OrderDto is not empty
        RuleFor(x => x.Order.Id)
            .NotEmpty()
            .WithMessage("Id is required");

        // Ensure the OrderName property of the OrderDto is not empty
        RuleFor(x => x.Order.OrderName)
            .NotEmpty()
            .WithMessage("Order name is required");

        // Ensure the CustomerId property of the OrderDto is not null
        RuleFor(x => x.Order.CustomerId)
            .NotNull()
            .WithMessage("Customer ID is required");
    }
}
