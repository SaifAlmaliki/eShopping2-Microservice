namespace Catalog.API.Products.DeleteProduct;

// Command to delete a product by its ID
public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

// Result of the delete product command
public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    // Constructor with dependency injection
    public DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    // Method to handle the delete command
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteProductCommand for Product ID: {ProductId}", command.Id);

        try
        {
            // Delete the product with the specified ID
            _session.Delete<Product>(command.Id);

            // Save changes to the data store
            await _session.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with ID: {ProductId} successfully deleted.", command.Id);

            // Return success result
            return new DeleteProductResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting Product with ID: {ProductId}", command.Id);
            throw;
        }
    }
}
