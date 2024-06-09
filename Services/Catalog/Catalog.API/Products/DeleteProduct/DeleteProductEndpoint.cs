namespace Catalog.API.Products.DeleteProduct;

// Record to represent the response for the delete operation
public record DeleteProductResponse(bool IsSuccess);

// Endpoint class for deleting a product
public class DeleteProductEndpoint : ICarterModule
{
    // Method to add routes to the app's routing table
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", HandleDeleteProduct)
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest) 
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product by specifying the ID.");
    }

    // Private method to handle the delete product request
    private static async Task<IResult> HandleDeleteProduct(Guid id, ISender sender)
    {
            // Send the command to delete the product
            var result = await sender.Send(new DeleteProductCommand(id));

            // Adapt the result to the response format
            var response = result.Adapt<DeleteProductResponse>();

            // Return the response with a 200 OK status
            return Results.Ok(response);
        

    }
}
