namespace Basket.API.Basket.GetBasket;

// Define response record for the get basket operation.
public record GetBasketResponse(ShoppingCart Cart);

// Define the GetBasketEndpoints class implementing the ICarterModule interface.
public class GetBasketEndpoints : ICarterModule
{
    // Method to add routes for the get basket functionality.
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", HandleGetBasket)
            .WithName("GetProductById")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
    }

    // Private static method to handle the get basket request.
    private static async Task<IResult> HandleGetBasket(string userName, ISender sender)
    {
        // Create the get basket query with the provided userName.
        var query = new GetBasketQuery(userName);

        // Send the query using the sender and await the result.
        var result = await sender.Send(query);

        // Adapt the result to a response.
        var response = result.Adapt<GetBasketResponse>();

        // Return an OK result with the response.
        return Results.Ok(response);
    }
}
