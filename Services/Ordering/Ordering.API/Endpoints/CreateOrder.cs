namespace Ordering.API.Endpoints;

// 1- Accepts a CreateOrderRequest object.
// 2- Maps the request to a CreateOrderCommand.
// 3- Uses MediatR to send the command to the corresponding handler.
// 4- Returns a response with the created order's ID.
//----------------------------------------------------------------------------------

// The CreateOrderRequest record represents the request payload for creating an order.
// It contains an OrderDto which includes all necessary details for the order.
public record CreateOrderRequest(OrderDto Order);

// The CreateOrderResponse record represents the response payload for the created order.
// It contains the unique identifier (GUID) of the created order.
public record CreateOrderResponse(Guid Id);

// The CreateOrder class defines an endpoint for creating orders.
// It implements the ICarterModule interface to integrate with Carter.
public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", HandleCreateOrder)
            .WithName("CreateOrder")            // Set the name for the endpoint
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created) // Define the response type and status code for successful creation
            .ProducesProblem(StatusCodes.Status400BadRequest) // Define the response type and status code for bad request
            .WithSummary("Create Order")        // Add a summary for the endpoint
            .WithDescription("Create Order");   // Add a description for the endpoint
    }

    // Handles the creation of a new order
    private static async Task<IResult> HandleCreateOrder(CreateOrderRequest request, ISender sender)
    {
        // Map the CreateOrderRequest to a CreateOrderCommand using the Mapster library
        var command = request.Adapt<CreateOrderCommand>();

        // Use MediatR to send the command to the corresponding handler and get the result
        var result = await sender.Send(command);

        // Map the result to a CreateOrderResponse using the Mapster library
        var response = result.Adapt<CreateOrderResponse>();

        // Return a Created response with the created order's ID and the response payload
        return Results.Created($"/orders/{response.Id}", response);
    }
}


