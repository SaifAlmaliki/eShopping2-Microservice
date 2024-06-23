// Purpose: Defines a strongly typed value object for Address to encapsulate address-related information and ensure consistency.

namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed Address.
// Records provide built-in functionality for value equality, meaning two Address instances with the same values are considered equal.
public record Address
{
    // Property to hold the first name of the individual.
    public string FirstName { get; } = default!;

    // Property to hold the last name of the individual.
    public string LastName { get; } = default!;

    // Property to hold the email address of the individual.
    // Nullable to allow for cases where an email address might not be provided.
    public string? EmailAddress { get; } = default!;

    // Property to hold the address line (e.g., street address).
    public string AddressLine { get; } = default!;

    // Property to hold the country of the address.
    public string Country { get; } = default!;

    // Property to hold the state or province of the address.
    public string State { get; } = default!;

    // Property to hold the zip or postal code of the address.
    public string ZipCode { get; } = default!;

    // Protected parameterless constructor for ORM tools or serialization purposes.
    protected Address()
    {
    }

    // Private constructor to ensure Address can only be created through the static method.
    // This enforces control over the creation process, ensuring all Address instances are valid.
    private Address(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }

    // Static method to create a new instance of Address.
    // Ensures that the provided email address and address line are not null or empty, enforcing validity constraints.
    public static Address Of(string firstName, string lastName, string emailAddress, string addressLine, string country, string state, string zipCode)
    {
        // Throws an exception if the provided email address is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress) checks if the value is null, empty, or whitespace and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);

        // Throws an exception if the provided address line is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(addressLine) checks if the value is null, empty, or whitespace and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);

        // Return a new instance of Address with the validated values.
        // This ensures that only valid values are used to create Address instances, preventing invalid data.
        return new Address(firstName, lastName, emailAddress, addressLine, country, state, zipCode);
    }
}
