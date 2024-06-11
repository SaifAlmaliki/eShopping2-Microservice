namespace Basket.API.Basket.StoreBasket;

// Define request and response records for the store basket operation.
public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);

// Define the StoreBasketEndpoints class implementing the ICarterModule interface.
public class StoreBasketEndpoints : ICarterModule
{
    // Method to add routes for the store basket functionality.
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", HandleStoreBasket)
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Stores a shopping basket for a user.");
    }
    private static async Task<IResult> HandleStoreBasket(StoreBasketRequest request, ISender sender)
    {
        // Adapt the request to a command.
        var command = request.Adapt<StoreBasketCommand>();

        // Send the command using the sender and await the result.
        var result = await sender.Send(command);

        // Adapt the result to a response.
        var response = result.Adapt<StoreBasketResponse>();

        // Return a Created result with the response.
        return Results.Created($"/basket/{response.UserName}", response);
    }
}