namespace Basket.API.Basket.CheckoutBasket;

// Command representing a request to checkout a basket.
// Contains a DTO with the necessary information for checkout.
public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<CheckoutBasketResult>;

// Result of the CheckoutBasketCommand indicating success or failure.
public record CheckoutBasketResult(bool IsSuccess);

// Validator for the CheckoutBasketCommand to ensure data integrity.
public class CheckoutBasketCommandValidator: AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        // Ensure BasketCheckoutDto is not null.
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");

        // Ensure UserName within BasketCheckoutDto is not empty.
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

// Handler for the CheckoutBasketCommand.
// Processes the command by interacting with the repository and publishing an event.
public class CheckoutBasketCommandHandler
    (IBasketRepository _repository, IPublishEndpoint _publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // Retrieve the existing basket for the given user.
        var basket = await _repository.GetBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);

        // If the basket does not exist, return a failure result.
        if (basket is null)
        {
            return new CheckoutBasketResult(false);
        }

        // Map the BasketCheckoutDto to a BasketCheckoutEvent and set the total price.
        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        // Publish the basket checkout event to RabbitMQ using MassTransit.
        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        // Delete the basket after successful checkout.
        await _repository.DeleteBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);

        // Return a success result.
        return new CheckoutBasketResult(true);
    }
}
