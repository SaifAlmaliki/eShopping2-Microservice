namespace Blazor.UI.Models.Basket;

// Represents the shopping cart model for a user
public class ShoppingCartModel
{
    // The username of the user who owns this shopping cart
    public string UserName { get; set; } = string.Empty;

    // A list of items in the shopping cart
    public List<ShoppingCartItemModel> Items { get; set; } = new();

    // Calculates the total price of all items in the cart
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}

// Represents an item in the shopping cart
public class ShoppingCartItemModel
{
    // Constructor to initialize a shopping cart item
    public ShoppingCartItemModel(int quantity, string color, decimal price, Guid productId, string productName)
    {
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductId = productId;
        ProductName = productName;
    }

    // The quantity of the product in the cart
    public int Quantity { get; set; }

    // The color of the product
    public string Color { get; set; }

    // The price of a single unit of the product
    public decimal Price { get; set; }

    // The unique identifier for the product
    public Guid ProductId { get; set; }

    // The name of the product
    public string ProductName { get; set; }
}

// Represents the response returned when getting the basket
public record GetBasketResponse(ShoppingCartModel Cart);

// Represents the request to store a basket
public record StoreBasketRequest(ShoppingCartModel Cart);

// Represents the response returned when storing the basket
public record StoreBasketResponse(string UserName);

// Represents the response returned when deleting the basket
public record DeleteBasketResponse(bool IsSuccess);
