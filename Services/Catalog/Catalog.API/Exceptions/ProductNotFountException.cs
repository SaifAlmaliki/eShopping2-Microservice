namespace Catalog.API.Exceptions;

// Custom exception class for handling "product not found" errors
public class ProductNotFoundException : NotFoundException
{
    // Constructor that accepts a product ID and passes a specific message to the base NotFoundException
    public ProductNotFoundException(Guid Id) : base("Product", Id)
    {
        // The base constructor is called with "Product" as the name and the "product ID" as the key.
        // This constructs the message "Entity 'Product' with key (Id) was not found."
    }
}
