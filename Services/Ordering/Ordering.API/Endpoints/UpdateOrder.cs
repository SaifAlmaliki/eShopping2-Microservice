namespace Ordering.API.Endpoints;

// The UpdateOrderRequest record represents the request payload for updating an order.
// It contains an OrderDto which includes all necessary details for the order.
public record UpdateOrderRequest(OrderDto Order);

// The UpdateOrderResponse record represents the response payload for updating an order.
// It contains a boolean indicating whether the update was successful.
public record UpdateOrderResponse(bool IsSuccess);

// The UpdateOrder class defines an endpoint for updating orders.
// It implements the ICarterModule interface to integrate with Carter.
public class UpdateOrder : ICarterModule
{
    // Method to add routes for the UpdateOrder endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", HandleUpdateOrder)
            .WithName("UpdateOrder")            // Set the name for the endpoint
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK) // Define the response type and status code for successful update
            .ProducesProblem(StatusCodes.Status400BadRequest)       // Define the response type and status code for bad request
            .WithSummary("Update Order")        // Add a summary for the endpoint
            .WithDescription("Update Order");   // Add a description for the endpoint
    }

    // Handles updating an order
    private static async Task<IResult> HandleUpdateOrder(UpdateOrderRequest request, ISender sender)
    {
        // Map the UpdateOrderRequest to an UpdateOrderCommand using the Mapster library
        var command = request.Adapt<UpdateOrderCommand>();

        // Use MediatR to send the command to the corresponding handler and get the result
        var result = await sender.Send(command);

        // Map the result to an UpdateOrderResponse using the Mapster library
        var response = result.Adapt<UpdateOrderResponse>();

        // Return an OK response with the update result
        return Results.Ok(response);
    }
}
