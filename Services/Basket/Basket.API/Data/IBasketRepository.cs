namespace Basket.API.Data;

/// <summary>
/// Interface for basket repository operations.
/// </summary>
public interface IBasketRepository
{
    /// <summary>
    /// Gets the shopping cart for a specified user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the shopping cart.</returns>
    Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stores the shopping cart.
    /// </summary>
    /// <param name="basket">The shopping cart to store.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the stored shopping cart.</returns>
    Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the shopping cart for a specified user.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the operation succeeded.</returns>
    Task<bool> DeleteBasketAsync(string username, CancellationToken cancellationToken = default);
}
