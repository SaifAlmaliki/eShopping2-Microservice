namespace Ordering.API.Endpoints;

//1- Accepts a customer ID.
//2- Uses a GetOrdersByCustomerQuery to fetch orders.
//3- Returns the list of orders for that customer.
//---------------------------------------------------

// The GetOrdersByCustomerResponse record represents the response payload for fetching orders by customer ID.
// It contains an IEnumerable of OrderDto representing the list of orders for the customer.
public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

// The GetOrdersByCustomer class defines an endpoint for fetching orders by customer ID.
// It implements the ICarterModule interface to integrate with Carter.
public class GetOrdersByCustomer : ICarterModule
{
    // Method to add routes for the GetOrdersByCustomer endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId}", HandleGetOrdersByCustomer)
            .WithName("GetOrdersByCustomer")                    // Set the name for the endpoint
            .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK) // Define the response type and status code for successful retrieval
            .ProducesProblem(StatusCodes.Status400BadRequest)   // Define the response type and status code for bad request
            .ProducesProblem(StatusCodes.Status404NotFound)     // Define the response type and status code for not found
            .WithSummary("Get Orders By Customer")              // Add a summary for the endpoint
            .WithDescription("Get Orders By Customer");         // Add a description for the endpoint
    }

    // Handles fetching orders by CustomerId
    private static async Task<IResult> HandleGetOrdersByCustomer(Guid customerId, ISender sender)
    {
        // Send the GetOrdersByCustomerQuery with the provided customer ID using MediatR
        var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

        // Map the result to a GetOrdersByCustomerResponse using the Mapster library
        var response = result.Adapt<GetOrdersByCustomerResponse>();

        // Return an OK response with the list of orders for the customer
        return Results.Ok(response);
    }
}
