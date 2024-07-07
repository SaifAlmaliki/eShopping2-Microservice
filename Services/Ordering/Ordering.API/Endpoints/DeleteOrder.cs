namespace Ordering.API.Endpoints;

//- Accepts the order ID as a parameter.
//- Constructs a DeleteOrderCommand.
//- Sends the command using MediatR.
//- Returns a success or not found response.
//-------------------------------------------------

// The DeleteOrderResponse record represents the response payload for deleting an order.
// It contains a boolean indicating whether the deletion was successful.
public record DeleteOrderResponse(bool IsSuccess);

// The DeleteOrder class defines an endpoint for deleting orders.
// It implements the ICarterModule interface to integrate with Carter.
public class DeleteOrder : ICarterModule
{
    // Method to add routes for the DeleteOrder endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Map the HTTP DELETE request to the "/orders/{id}" endpoint
        app.MapDelete("/orders/{id}", async (Guid Id, ISender sender) =>
        {
            // Construct a DeleteOrderCommand with the provided order ID
            var result = await sender.Send(new DeleteOrderCommand(Id));

            // Map the result to a DeleteOrderResponse using the Mapster library
            var response = result.Adapt<DeleteOrderResponse>();

            // Return an OK response with the deletion result
            return Results.Ok(response);
        })
        .WithName("DeleteOrder")        // Set the name for the endpoint
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK) // Define the response type and status code for successful deletion
        .ProducesProblem(StatusCodes.Status400BadRequest)   // Define the response type and status code for bad request
        .ProducesProblem(StatusCodes.Status404NotFound)     // Define the response type and status code for not found
        .WithSummary("Delete Order")        // Add a summary for the endpoint
        .WithDescription("Delete Order");   // Add a description for the endpoint
    }
}

