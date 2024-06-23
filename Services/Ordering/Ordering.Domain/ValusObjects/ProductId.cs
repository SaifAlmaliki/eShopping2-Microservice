// Purpose: Defines a strongly typed value object for ProductId to avoid mistakes and ensure consistency.

namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed ProductId.
// Records provide built-in functionality for value equality, meaning two ProductId instances with the same value are considered equal.
public record ProductId
{
    // Property to hold the GUID value of the ProductId.
    // This encapsulates the unique identifier for a product.
    public Guid Value { get; }

    // Private constructor to ensure ProductId can only be created through the static method.
    // This enforces control over the creation process, ensuring all ProductId instances are valid.
    private ProductId(Guid value) => Value = value;

    // Static method to create a new instance of ProductId.
    // Ensures that the provided GUID is not null or empty, enforcing validity constraints.
    public static ProductId Of(Guid value)
    {
        // ArgumentNullException.ThrowIfNull(value) checks if the value is null and throws an ArgumentNullException if it is.
        // This ensures that the method does not proceed with a null GUID, which would be invalid.
        ArgumentNullException.ThrowIfNull(value);

        // Check if the provided GUID is empty (default GUID value is Guid.Empty).
        // An empty GUID is considered invalid because it does not represent a unique identifier.
        if (value == Guid.Empty)
        {
            // If the value is empty, a DomainException is thrown to indicate that an empty GUID is not allowed.
            // DomainException is a custom exception that provides a specific error message for domain-related validation errors.
            throw new DomainException("ProductId cannot be empty.");
        }

        // Return a new instance of ProductId with the validated GUID value.
        // This ensures that only valid GUIDs are used to create ProductId instances, preventing invalid data.
        return new ProductId(value);
    }
}
