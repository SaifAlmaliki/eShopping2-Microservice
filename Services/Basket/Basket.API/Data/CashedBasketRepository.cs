namespace Basket.API.Data;

// Class declaration with primary constructor parameters.
// These parameters are automatically assigned to private fields.
public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    // Deletes a user's basket asynchronously.
    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        // First, delete the basket from the primary repository.
        await repository.DeleteBasketAsync(userName, cancellationToken);

        // Then, remove the basket from the distributed cache.
        await cache.RemoveAsync(userName, cancellationToken);

        // Return true to indicate the basket was successfully deleted.
        return true;
    }

    // Retrieves a user's basket asynchronously.
    public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken = default)
    {
        // Try to get the basket from the distributed cache.
        var cachedBasket = await cache.GetStringAsync(username, cancellationToken);

        // If the basket is found in the cache, deserialize and return it.
        if (!string.IsNullOrEmpty(cachedBasket))
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

        // If not found in the cache, retrieve it from the primary repository.
        var basket = await repository.GetBasketAsync(username, cancellationToken);

        // Store the retrieved basket in the distributed cache for future requests.
        await cache.SetStringAsync(username, JsonSerializer.Serialize(basket), cancellationToken);

        // Return the retrieved basket.
        return basket;
    }

    // Stores a user's basket asynchronously.
    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        // First, store the basket in the primary repository.
        await repository.StoreBasketAsync(basket, cancellationToken);

        // Then, store the serialized basket in the distributed cache.
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

        // Return the stored basket.
        return basket;
    }
}
