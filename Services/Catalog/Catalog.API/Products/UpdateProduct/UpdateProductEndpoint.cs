namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    // Method to add routes to the app's routing table
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",HandleUpdateProductAsync)
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product by specifying the details.");
    }

    // Private method to handle the update product request
    private async Task<IResult> HandleUpdateProductAsync(UpdateProductRequest request, ISender sender)
    {
        // Adapt the request to the command format
        var command = request.Adapt<UpdateProductCommand>();

        // Send the command to update the product
        var result = await sender.Send(command);

        // Adapt the result to the response format
        var response = result.Adapt<UpdateProductResponse>();

        // Log the success and return a 200 OK status with the response
        return Results.Ok(response);
    }
}
