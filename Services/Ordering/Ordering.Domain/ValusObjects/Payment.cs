namespace Ordering.Domain.ValueObjects;

// Record representing a strongly typed Payment.
// Records provide built-in functionality for value equality, meaning two Payment instances with the same values are considered equal.
public record Payment
{
    // Property to hold the name on the card.
    // Nullable to allow for cases where a card name might not be provided.
    public string? CardName { get; } = default!;

    // Property to hold the card number.
    public string CardNumber { get; } = default!;

    // Property to hold the expiration date of the card.
    public string Expiration { get; } = default!;

    // Property to hold the CVV (Card Verification Value) of the card.
    public string CVV { get; } = default!;

    // Property to hold the payment method identifier.
    public int PaymentMethod { get; } = default!;

    // Protected parameterless constructor for ORM tools or serialization purposes.
    protected Payment()
    {
    }

    // Private constructor to ensure Payment can only be created through the static method.
    // This enforces control over the creation process, ensuring all Payment instances are valid.
    private Payment(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    // Static method to create a new instance of Payment.
    // Ensures that the provided card information and payment method are valid, enforcing various constraints.
    public static Payment Of(string cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
    {
        // Throws an exception if the provided card name is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(cardName) checks if the value is null, empty, or whitespace and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(cardName);

        // Throws an exception if the provided card number is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber) checks if the value is null, empty, or whitespace and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber);

        // Throws an exception if the provided CVV is null, empty, or consists only of white-space characters.
        // ArgumentException.ThrowIfNullOrWhiteSpace(cvv) checks if the value is null, empty, or whitespace and throws an ArgumentException if it is.
        ArgumentException.ThrowIfNullOrWhiteSpace(cvv);

        // Throws an exception if the provided CVV length is greater than 3 characters.
        // ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3) checks if the length of CVV is more than 3 and throws an ArgumentOutOfRangeException if it is.
        ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length, 3);

        // Return a new instance of Payment with the validated values.
        // This ensures that only valid values are used to create Payment instances, preventing invalid data.
        return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
    }
}
