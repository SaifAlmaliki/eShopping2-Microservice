namespace Catalog.API.Products.CreateProduct;

// Define a command to create a product with necessary details
public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;

// Define a result that includes the ID of the newly created product
public record CreateProductResult(Guid Id);

// Validator for the CreateProductCommand to ensure all necessary fields are correctly populated
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

// Command handler to process the CreateProductCommand
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
        try
        {
            _logger.LogInformation("Starting the product creation process for product: {ProductName}", command.Name);

            // Create a new product entity from the command details
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            _logger.LogInformation("Product entity created with Name: {Name}, Category: {Category}, Description: {Description}, ImageFile: {ImageFile}, Price: {Price}",
               product.Name, string.Join(", ", product.Category), product.Description, product.ImageFile, product.Price);

            // Save the product entity to the database
            _session.Store(product);
            await _session.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product saved successfully to the database with ID: {ProductId}", product.Id);

            // Return the result containing the new product ID
            return new CreateProductResult(product.Id);
        }
        catch (Exception ex)
        {
            // Log any errors that occur during the process
            _logger.LogError(ex, "Error occurred while creating product: {ProductName}", command.Name);
            throw;
        }
    }
}
