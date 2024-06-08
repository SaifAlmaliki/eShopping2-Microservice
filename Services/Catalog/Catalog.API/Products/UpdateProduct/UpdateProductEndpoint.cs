namespace Catalog.API.Products.UpdateProduct;

// Record to represent the request for updating a product
public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

// Record to represent the response for the update operation
public record UpdateProductResponse(bool IsSuccess);

// Endpoint class for updating a product
public class UpdateProductEndpoint : ICarterModule
{
    // Method to add routes to the app's routing table
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", HandleUpdateProduct)
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product") 
            .WithDescription("Update Product by specifying the details.");
    }

    // Private method to handle the update product request
    private static async Task<IResult> HandleUpdateProduct(UpdateProductRequest request, ISender sender, ILogger<UpdateProductEndpoint> logger)
    {
        logger.LogInformation("Handling update for Product ID: {ProductId}", request.Id);

        try
        {
            // Adapt the request to the command format
            var command = request.Adapt<UpdateProductCommand>();

            // Send the command to update the product
            var result = await sender.Send(command);

            // Adapt the result to the response format
            var response = result.Adapt<UpdateProductResponse>();

            // Return the response with a 200 OK status
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during the process
            logger.LogError(ex, "Error occurred while updating Product with ID: {ProductId}", request.Id);
            return Results.Problem("An error occurred while updating the product.");
        }
    }
}
