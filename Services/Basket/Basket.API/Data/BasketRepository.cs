namespace Basket.API.Data;

// Implementation of basket repository using IDocumentSession.
public class BasketRepository(IDocumentSession session) : IBasketRepository
{

    // Method to delete a basket asynchronously.
    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(cancellationToken);
        return true;
    }

    // Method to get a basket asynchronously.
    public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken = default)
    {
        var basket = await session.LoadAsync<ShoppingCart>(username, cancellationToken);
        return basket ?? throw new BasketNotFoundException(username);
    }

    // Method to store a basket asynchronously.
    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        session.Store(basket);
        await session.SaveChangesAsync(cancellationToken);
        return basket;
    }
}
