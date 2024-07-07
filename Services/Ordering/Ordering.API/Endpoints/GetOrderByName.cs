namespace Ordering.API.Endpoints;

//- Accepts a name parameter.
//- Constructs a GetOrdersByNameQuery.
//- Retrieves and returns matching orders.

//-----------------------------------------
// The GetOrdersByNameResponse record represents the response payload for fetching orders by name.
// It contains an IEnumerable of OrderDto representing the list of orders matching the name.
public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

// The GetOrdersByName class defines an endpoint for fetching orders by name.
// It implements the ICarterModule interface to integrate with Carter.
public class GetOrdersByName : ICarterModule
{
    // Method to add routes for the GetOrdersByName endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", HandleGetOrdersByName)
            .WithName("GetOrdersByName") // Set the name for the endpoint
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK) // Define the response type and status code for successful retrieval
            .ProducesProblem(StatusCodes.Status400BadRequest)   // Define the response type and status code for bad request
            .ProducesProblem(StatusCodes.Status404NotFound)     // Define the response type and status code for not found
            .WithSummary("Get Orders By Name")          // Add a summary for the endpoint
            .WithDescription("Get Orders By Name");     // Add a description for the endpoint
    }

    // Handles fetching orders by name
    private static async Task<IResult> HandleGetOrdersByName(string orderName, ISender sender)
    {
        // Send the GetOrdersByNameQuery with the provided order name using MediatR
        var result = await sender.Send(new GetOrdersByNameQuery(orderName));

        // Map the result to a GetOrdersByNameResponse using the Mapster library
        var response = result.Adapt<GetOrdersByNameResponse>();

        // Return an OK response with the list of orders matching the name
        return Results.Ok(response);
    }
}

