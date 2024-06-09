namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    private readonly ILogger<UpdateProductEndpoint> _logger;

    public UpdateProductEndpoint(ILogger<UpdateProductEndpoint> logger)
    {
        _logger = logger;
    }

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
        try
        {
            // Adapt the request to the command format
            var command = request.Adapt<UpdateProductCommand>();

            // Send the command to update the product
            var result = await sender.Send(command);

            // Adapt the result to the response format
            var response = result.Adapt<UpdateProductResponse>();

            // Log the success and return a 200 OK status with the response
            _logger.LogInformation("Product '{ProductName}' updated successfully with ID: {ProductId}", command.Name, request.Id);
            return Results.Ok(response);
        }
        catch (ValidationException ex)
        {
            // Log validation errors
            _logger.LogError(ex, "Validation failed for product update request: {Request}", request);

            // Return a 400 Bad Request status with detailed validation error information
            var errorDetails = new
            {
                Message = "Validation failed for one or more fields.",
                Errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage, e.Severity })
            };

            return Results.BadRequest(errorDetails);
        }
        catch (Exception ex)
        {
            // Log unexpected errors
            _logger.LogError(ex, "An unexpected error occurred while updating product: {Request}", request);

            // Return a 500 Internal Server Error status with error details
            var errorDetails = new
            {
                Message = "An unexpected error occurred while processing your request.",
                Error = ex.Message
            };

            return Results.StatusCode(500);
        }
    }
}
