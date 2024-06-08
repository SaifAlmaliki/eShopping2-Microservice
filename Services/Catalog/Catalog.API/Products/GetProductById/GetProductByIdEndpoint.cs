namespace Catalog.API.Products.GetProductById;

// Response record for GetProductById endpoint
public record GetProductByIdResponse(Product Product);

// Endpoint class implementing ICarterModule
public class GetProductByIdEndpoint : ICarterModule
{
    // Method to add routes to the endpoint
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Map GET request for getting product by id
        app.MapGet("/products/{id}", HandleGetProductByIdAsync)
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
    }

    // Async handler for GET request to get product by id
    private static async Task<IResult> HandleGetProductByIdAsync(Guid id, ISender sender)
    {
        // Send query to get product by id
        var result = await sender.Send(new GetProductByIdQuery(id));

        // Adapt result to response type
        var response = result.Adapt<GetProductByIdResponse>();

        // Return OK result with response
        return Results.Ok(response);
    }
}
