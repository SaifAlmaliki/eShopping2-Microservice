namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed OrderId.
// Records provide built-in functionality for value equality.
public record OrderId
{
    // Property to hold the GUID value of the OrderId.
    public Guid Value { get; }

    // Private constructor to ensure OrderId can only be created through the static method.
    private OrderId(Guid value) => Value = value;

    // Static method to create a new instance of OrderId.
    // This method serves as a factory method to ensure the creation of valid OrderId instances.
    public static OrderId Of(Guid value)
    {
        // Throws an exception if the provided GUID is null.
        // ArgumentNullException.ThrowIfNull(value) checks if the value is null and throws an ArgumentNullException if it is.
        ArgumentNullException.ThrowIfNull(value);

        // Check if the provided GUID is empty (default GUID value is Guid.Empty).
        // If the value is empty, a DomainException is thrown to indicate that an empty GUID is not allowed.
        if (value == Guid.Empty)
        {
            throw new DomainException("OrderId cannot be empty.");
        }

        // Return a new instance of OrderId with the validated GUID value.
        // This ensures that only valid GUIDs are used to create OrderId instances, preventing invalid data.
        return new OrderId(value);
    }
}