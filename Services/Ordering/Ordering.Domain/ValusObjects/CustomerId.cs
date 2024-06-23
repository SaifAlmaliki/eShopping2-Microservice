// Purpose: Defines a strongly typed value object for CustomerId to avoid mistakes and ensure consistency.

namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed CustomerId.
// Records provide built-in functionality for value equality, meaning two CustomerId instances with the same value are considered equal.
public record CustomerId
{
    // Property to hold the GUID value of the CustomerId.
    // This encapsulates the unique identifier for a customer.
    public Guid Value { get; }

    // Private constructor to ensure CustomerId can only be created through the static method.
    // This enforces control over the creation process, ensuring all CustomerId instances are valid.
    private CustomerId(Guid value) => Value = value;

    // Static method to create a new instance of CustomerId.
    // Ensures that the provided GUID is not null or empty, enforcing validity constraints.
    public static CustomerId Of(Guid value)
    {
        // Throws an exception if the provided GUID is null.
        // ArgumentNullException.ThrowIfNull(value) checks if the value is null and throws an ArgumentNullException if it is.
        // This ensures that the method does not proceed with a null GUID, which would be invalid.
        ArgumentNullException.ThrowIfNull(value);

        // Check if the provided GUID is empty (default GUID value is Guid.Empty).
        // An empty GUID is considered invalid because it does not represent a unique identifier.
        if (value == Guid.Empty)
        {
            // If the value is empty, a DomainException is thrown to indicate that an empty GUID is not allowed.
            // DomainException is a custom exception that provides a specific error message for domain-related validation errors.
            throw new DomainException("CustomerId cannot be empty.");
        }

        // Return a new instance of CustomerId with the validated GUID value.
        // This ensures that only valid GUIDs are used to create CustomerId instances, preventing invalid data.
        return new CustomerId(value);
    }
}
