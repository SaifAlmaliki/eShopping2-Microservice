namespace Basket.API.Basket.DeleteBasket
{
    // Define a command record for deleting a basket, containing a single property UserName.
    public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

    // Define a result record for the delete basket operation, containing a single property IsSuccess.
    public record DeleteBasketResult(bool IsSuccess);

    // Define a validator for the DeleteBasketCommand using FluentValidation.
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        // Constructor where validation rules are defined.
        public DeleteBasketCommandValidator()
        {
            // Ensure UserName is not empty, with a custom error message.
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    // Define a handler for the DeleteBasketCommand, implementing ICommandHandler interface.
    public class DeleteBasketCommandHandler
        (IBasketRepository repository)
        : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        // Handle method to process the DeleteBasketCommand.
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            // Call the repository method to delete the basket asynchronously.
            await repository.DeleteBasketAsync(command.UserName, cancellationToken);

            // Return a successful result.
            return new DeleteBasketResult(true);
        }
    }
}
