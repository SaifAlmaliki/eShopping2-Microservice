namespace Blazor.UI.Services;
public interface IBasketService
{
    // Retrieves the basket for a given user by username
    // GET request to /basket-service/basket/{userName}
    [Get("/basket-service/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    // Stores the basket for a user
    // POST request to /basket-service/basket
    [Post("/basket-service/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request);

    // Deletes the basket for a given user by username
    // DELETE request to /basket-service/basket/{userName}
    [Delete("/basket-service/basket/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    // Checks out the basket for a user
    // POST request to /basket-service/basket/checkout
    [Post("/basket-service/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

    // Loads the user's basket, creating a new one if it does not exist
    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        // Get Basket If Not Exist Create New Basket with Default Logged In User Name: swn
        var userName = "swn";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}

/* 
* This file defines the IBasketService interface and its methods for interacting
* with a basket service in a Blazor application. The service includes methods 
* for getting, storing, deleting, and checking out a user's basket. Additionally, 
* it provides a method for loading a user's basket, creating a new one if it does not exist.
*/