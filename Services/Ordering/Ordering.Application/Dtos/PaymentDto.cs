namespace Ordering.Application.Dtos;

// The PaymentDto represents payment details for an order.
// This DTO is necessary for capturing and transferring payment information securely within the application.
public record PaymentDto(
    string CardName,
    string CardNumber,
    string Expiration,
    string Cvv,
    int PaymentMethod);
