// Purpose: Defines a strongly typed value object for OrderName to avoid mistakes and ensure consistency.

namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed OrderName.
// Records provide built-in functionality for value equality, meaning two OrderName instances with the same value are considered equal.
public record OrderName
{
    // Constant defining the default length for an OrderName.
    private const int DefaultLength = 5;

    // Property to hold the string value of the OrderName.
    // This encapsulates the name of an order.
    public string Value { get; }

    // Private constructor to ensure OrderName can only be created through the static method.
    // This enforces control over the creation process, ensuring all OrderName instances are valid.
    private OrderName(string value) => Value = value;

    // Static method to create a new instance of OrderName.
    // Ensures that the provided string is not null, empty, or whitespace.
    public static OrderName Of(string value)
    {
        // Throws an exception if the provided string is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(value) checks if the value is null or white space and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        // Return a new instance of OrderName with the validated string value.
        // This ensures that only valid strings are used to create OrderName instances, preventing invalid data.
        return new OrderName(value);
    }
}
