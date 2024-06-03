// This file defines models and wrapper classes used in the ordering process
// for a Blazor application. These models represent orders, order items, 
// addresses, payments, and order statuses. The wrapper classes are used 
// to encapsulate responses for various order-related operations.

namespace Blazor.UI.Models.Ordering;

// Represents an order with its details
public record OrderModel(
    Guid Id,                        // The unique identifier of the order
    Guid CustomerId,                // The unique identifier of the customer who placed the order
    string OrderName,               // The name of the order
    AddressModel ShippingAddress,   // The shipping address for the order
    AddressModel BillingAddress,    // The billing address for the order
    PaymentModel Payment,           // The payment details for the order
    OrderStatus Status,             // The current status of the order
    List<OrderItemModel> OrderItems // The list of items in the order
);

// Represents an item in an order
public record OrderItemModel(
    Guid OrderId,      // The unique identifier of the order
    Guid ProductId,    // The unique identifier of the product
    int Quantity,      // The quantity of the product in the order
    decimal Price      // The price of the product
);

// Represents an address used for shipping or billing
public record AddressModel(
    string FirstName,      // The first name of the person at the address
    string LastName,       // The last name of the person at the address
    string EmailAddress,   // The email address of the person at the address
    string AddressLine,    // The address line (e.g., street address)
    string Country,        // The country of the address
    string State,          // The state of the address
    string ZipCode         // The zip code of the address
);

// Represents payment details for an order
public record PaymentModel(
    string CardName,       // The name on the payment card
    string CardNumber,     // The number of the payment card
    string Expiration,     // The expiration date of the payment card
    string Cvv,            // The CVV code of the payment card
    int PaymentMethod      // The payment method (e.g., credit card, PayPal)
);

// Represents the status of an order
public enum OrderStatus
{
    Draft = 1,      // Order is in draft state
    Pending = 2,    // Order is pending
    Completed = 3,  // Order is completed
    Cancelled = 4   // Order is cancelled
}

// wrapper classes represent responses from an API
public record GetOrdersResponse(PaginatedResult<OrderModel> Orders);
public record GetOrdersByNameResponse(IEnumerable<OrderModel> Orders);
public record GetOrdersByCustomerResponse(IEnumerable<OrderModel> Orders);
