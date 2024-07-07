namespace Ordering.Application.Dtos;

// The AddressDto represents an address with various details.
// This DTO is essential for capturing the shipping and billing addresses in the order.
public record AddressDto(
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode);
