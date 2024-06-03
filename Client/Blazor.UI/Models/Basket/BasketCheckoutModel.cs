namespace Blazor.UI.Models.Basket;

// Represents the model used for the checkout process of the shopping basket
public class BasketCheckoutModel
{
    // Constructor to initialize required fields
    public BasketCheckoutModel(
        string userName,
        Guid customerId,
        decimal totalPrice,
        string firstName,
        string lastName,
        string emailAddress,
        string addressLine,
        string country,
        string state,
        string zipCode,
        string cardName,
        string cardNumber,
        string expiration,
        string cvv,
        int paymentMethod)
    {
        UserName = userName;
        CustomerId = customerId;
        TotalPrice = totalPrice;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
        CardName = cardName;
        CardNumber = cardNumber;
        Expiration = expiration;
        CVV = cvv;
        PaymentMethod = paymentMethod;
    }

    // The username of the customer checking out
    public string UserName { get; }

    // The unique identifier of the customer
    public Guid CustomerId { get; }

    // The total price of the items in the basket
    public decimal TotalPrice { get; }

    // Shipping and Billing Address Information
    public string FirstName { get; }
    public string LastName { get; }
    public string EmailAddress { get; }
    public string AddressLine { get; }
    public string Country { get; }
    public string State { get; }
    public string ZipCode { get; }

    // Payment Information
    public string CardName { get; }
    public string CardNumber { get; }
    public string Expiration { get; }
    public string CVV { get; }

    // The payment method selected by the customer
    public int PaymentMethod { get; }
}

// Wrapper class representing the request to checkout a basket
public record CheckoutBasketRequest(BasketCheckoutModel BasketCheckoutDto);

// Wrapper class representing the response after attempting to checkout a basket
public record CheckoutBasketResponse(bool IsSuccess);
