namespace Ordering.Domain.Models;
public class Product : Entity<ProductId>
{
    // Property to store the name of the product.
    // The 'private set' modifier means it can only be set within this class.
    public string Name { get; private set; } = default!;

    // Property to store the price of the product.
    // The 'private set' modifier means it can only be set within this class.
    public decimal Price { get; private set; } = default!;

    // Static method to create a new 'Product' instance.
    // It takes an ID, name, and price as parameters.
    public static Product Create(ProductId id, string name, decimal price)
    {
        // Throws an exception if the 'name' parameter is null or whitespace.
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        // Throws an exception if the 'price' parameter is negative or zero.
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        // Creates a new 'Product' instance and sets its properties.
        var product = new Product
        {
            Id = id,
            Name = name,
            Price = price
        };

        // Returns the newly created 'Product' instance.
        return product;
    }
}
