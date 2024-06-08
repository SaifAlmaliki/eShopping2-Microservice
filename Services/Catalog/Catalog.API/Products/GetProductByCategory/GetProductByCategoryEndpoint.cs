namespace Catalog.API.Products.GetProductByCategory;

// Record to represent the response containing a collection of products
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    // Method to add routes to the app's routing table
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", HandleGetProductsByCategory)
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest) 
            .WithSummary("Get Product By Category")
            .WithDescription("Get products by specifying a category.");
    }

    // Private method to handle the logic for getting products by category
    private async Task<IResult> HandleGetProductsByCategory(string category, ISender sender)
    {
        // Send the query to get products by category
        var result = await sender.Send(new GetProductByCategoryQuery(category));

        // Adapt the result to the response format
        var response = result.Adapt<GetProductByCategoryResponse>();

        // Return the response with a 200 OK status
        return Results.Ok(response);
    }
}