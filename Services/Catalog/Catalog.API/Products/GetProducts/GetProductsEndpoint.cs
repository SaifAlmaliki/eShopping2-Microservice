// The purpose of this code is to handle the retrieval of products from the catalog system.
// It defines the request and response models, as well as the endpoint to process the retrieval request
// and return the list of products.

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
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            // Adapt the request model to a query model for retrieving products
            var query = request.Adapt<GetProductsQuery>();

            // Send the query to get the result
            var result = await sender.Send(query);

            // Adapt the result to a response model
            var response = result.Adapt<GetProductsResponse>();

            // Return an OK result with the response model
            return Results.Ok(response);
        })
        // Provide additional metadata for the endpoint
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
