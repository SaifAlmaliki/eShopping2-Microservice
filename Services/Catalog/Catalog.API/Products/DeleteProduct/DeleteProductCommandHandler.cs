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

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;

    // Constructor with dependency injection
    public DeleteProductCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    // Method to handle the delete command
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
    
        // Delete the product with the specified ID
        _session.Delete<Product>(command.Id);

        // Save changes to the data store
        await _session.SaveChangesAsync(cancellationToken);

        // Return success result
        return new DeleteProductResult(true);
    }
}
