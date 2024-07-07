namespace Ordering.API.Endpoints;

//1- Accepts pagination parameters.
//2- Constructs a GetOrdersQuery with these parameters.
//3- Retrieves the data and returns it in a paginated format.
//-------------------------------------------------------

// The GetOrdersResponse record represents the response payload for fetching orders with pagination.
// It contains a PaginatedResult of OrderDto representing the paginated list of orders.
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

// The GetOrders class defines an endpoint for fetching orders with pagination.
// It implements the ICarterModule interface to integrate with Carter.
public class GetOrders : ICarterModule
{
    // Method to add routes for the GetOrders endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", HandleGetOrders)
            .WithName("GetOrders")      // Set the name for the endpoint
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)   // Define the response type and status code for successful retrieval
            .ProducesProblem(StatusCodes.Status400BadRequest)       // Define the response type and status code for bad request
            .ProducesProblem(StatusCodes.Status404NotFound)         // Define the response type and status code for not found
            .WithSummary("Get Orders")      // Add a summary for the endpoint
            .WithDescription("Get Orders"); // Add a description for the endpoint
    }

    // Handles fetching orders with pagination
    // [AsParameters] attribute treats the properties of PaginationRequest as individual query parameters
    private static async Task<IResult> HandleGetOrders([AsParameters] PaginationRequest request, ISender sender)
    {
        // Send the GetOrdersQuery with the provided pagination request using MediatR
        var result = await sender.Send(new GetOrdersQuery(request));

        // Map the result to a GetOrdersResponse using the Mapster library
        var response = result.Adapt<GetOrdersResponse>();

        // Return an OK response with the paginated list of orders
        return Results.Ok(response);
    }
}
