namespace Basket.API.Basket.GetBasket;

// Define a query record for getting a basket, containing a single property UserName.
public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

// Define a result record for the get basket operation, containing a single property Cart.
public record GetBasketResult(ShoppingCart Cart);

// Define a handler for the GetBasketQuery, implementing IQueryHandler interface.
public class GetBasketQueryHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    // Handle method to process the GetBasketQuery.
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        // Fetch the basket asynchronously from the repository using the provided UserName.
        var basket = await repository.GetBasketAsync(query.UserName);

        // Return the fetched basket wrapped in a GetBasketResult.
        return new GetBasketResult(basket);
    }
}