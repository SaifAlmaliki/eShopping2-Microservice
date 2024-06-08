namespace Catalog.API.Products.UpdateProduct;

// Command to update a product with the given details
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;

// Result of the update product command
public record UpdateProductResult(bool IsSuccess);

// Handler class for processing the UpdateProductCommand
internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session;  // Session to interact with the data store
    private readonly ILogger<UpdateProductCommandHandler> _logger;  // Logger instance

    // Constructor with dependency injection
    public UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    // Method to handle the update command
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateProductCommand for Product ID: {ProductId}", command.Id);

        try
        {
            // Load the product with the specified ID
            var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product is null)
            {
                // Log and throw if the product is not found
                _logger.LogWarning("Product with ID: {ProductId} not found.", command.Id);
                throw new ProductNotFoundException(command.Id);
            }

            // Update the product properties
            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            // Update the product in the session
            _session.Update(product);

            // Save changes to the data store
            await _session.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with ID: {ProductId} successfully updated.", command.Id);

            // Return success result
            return new UpdateProductResult(true);
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during the update process
            _logger.LogError(ex, "Error occurred while updating Product with ID: {ProductId}", command.Id);
            throw;
        }
    }
}
