namespace Basket.API.Basket.DeleteBasket;

// Define response records for the delete basket operation.
public record DeleteBasketResponse(bool IsSuccess);

// Define the DeleteBasketEndpoints class implementing the ICarterModule interface.
public class DeleteBasketEndpoints : ICarterModule
{
    // Method to add routes for the delete basket functionality.
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", HandleDeleteBasket)
            .WithName("DeleteProduct")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Deletes a user's basket.");
    }

    // Private static method to handle the delete basket request.
    private static async Task<IResult> HandleDeleteBasket(string userName, ISender sender)
    {
        // Adapt the username to a delete basket command.
        var command = new DeleteBasketCommand(userName);

        // Send the command using the sender and await the result.
        var result = await sender.Send(command);

        // Adapt the result to a response.
        var response = result.Adapt<DeleteBasketResponse>();

        // Return an OK result with the response.
        return Results.Ok(response);
    }
}
