namespace Catalog.API.Products.UpdateProduct;

// Command to update a product with the given details
public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<UpdateProductResult>;

// Result of the update product command
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

// Handler class for processing the UpdateProductCommand
internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session;

    // Constructor with dependency injection
    public UpdateProductCommandHandler(IDocumentSession session)
    {
        _session = session;
    }

    // Method to handle the update command
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Load the product with the specified ID
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
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

        // Return success result
        return new UpdateProductResult(true);
    }
}
