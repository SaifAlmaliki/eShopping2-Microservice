// The purpose of this code is to handle the creation of a new product in the catalog system.
// It defines the command and result models, as well as the handler to process the creation of the product
// and save it to the database.

namespace Catalog.API.Products.CreateProduct;

// Define a command to create a product with necessary details
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

// Define a result that includes the ID of the newly created product
public record CreateProductResult(Guid Id);

// Define a command handler to process the CreateProductCommand
internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    // Constructor to initialize the session and logger
    public CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Handle method to process the command and return the result
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting the product creation process for product: {ProductName}", command.Name);

        var product = CreateProductEntity(command);

        _logger.LogDebug("Product entity created with Name: {Name}, Category: {Category}, Description: {Description}, ImageFile: {ImageFile}, Price: {Price}",
            product.Name, string.Join(", ", product.Category), product.Description, product.ImageFile, product.Price);

        // Save the product entity to the database
        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product saved succussfully to the database with ID: {ProductId}", product.Id);

        return new CreateProductResult(product.Id);
    }



    // Create a Product entity from the command object
    private static Product CreateProductEntity(CreateProductCommand command)
    {
        return new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };
    }
}
