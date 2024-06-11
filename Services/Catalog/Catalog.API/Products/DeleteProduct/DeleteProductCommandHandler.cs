namespace Catalog.API.Products.DeleteProduct;

// Command to delete a product by its ID
public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

// Result of the delete product command
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
    }
}

// Handler class for processing the DeleteProductCommandHandler
// Uses a primary constructor to inject the IDocumentSession dependency
internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    // Method to handle the delete command
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        // Delete the product with the specified ID
        session.Delete<Product>(command.Id);

        // Save changes to the data store
        await session.SaveChangesAsync(cancellationToken);

        // Return success result
        return new DeleteProductResult(true);
    }
}
