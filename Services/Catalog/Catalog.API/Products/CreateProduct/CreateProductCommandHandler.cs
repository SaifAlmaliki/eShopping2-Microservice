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
// Uses a primary constructor to inject the IDocumentSession dependency
internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    // Handle method to process the command and return the result
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create a new product entity from the command details
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // Save the product entity to the database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        // Return the result containing the new product ID
        return new CreateProductResult(product.Id);
    }
}
