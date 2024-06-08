namespace Catalog.API.Products.GetProducts;

// Define a request model to specify pagination parameters for retrieving products
public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);

// Define a response model to return a collection of products
public record GetProductsResponse(IEnumerable<Product> Products);

// Define an endpoint to handle GET requests for retrieving products
public class GetProductsEndpoint : ICarterModule
{
    // Method to add routes to the application
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Map a GET request to the /products endpoint
        app.MapGet("/products", HandleGetProductsAsync)
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK) // Specify the successful response type and status code
            .ProducesProblem(StatusCodes.Status400BadRequest)       // Specify the error response type and status code
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }

    // Async handler for GET request to retrieve products
    private static async Task<IResult> HandleGetProductsAsync([AsParameters] GetProductsRequest request, ISender sender)
    {
        try
        {
            // Adapt the request model to a query model for retrieving products
            var query = request.Adapt<GetProductsQuery>();

            // Send the query to get the result
            var result = await sender.Send(query);

            // Adapt the result to a response model
            var response = result.Adapt<GetProductsResponse>();

            // Return an OK result with the response model
            return Results.Ok(response);
        }
        catch (Exception)
        {
            return Results.Problem("An error occurred while retrieving products.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
