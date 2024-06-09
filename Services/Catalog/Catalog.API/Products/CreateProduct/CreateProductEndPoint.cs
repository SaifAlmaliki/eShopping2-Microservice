namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);

// Class implementing the ICarterModule interface to define endpoint routes
public class CreateProductEndPoint : ICarterModule
{
    private readonly ILogger<CreateProductEndPoint> _logger;

    public CreateProductEndPoint(ILogger<CreateProductEndPoint> logger)
    {
        _logger = logger;
    }

    // Method to add routes to the endpoint route builder
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Define a POST endpoint at "/products"
        app.MapPost("/products", HandleCreateProductAsync)
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created) // Specify successful response type and status code
            .ProducesProblem(StatusCodes.Status400BadRequest) // Specify error response type and status code
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
    
    // Async handler for POST request to create a product
    private async Task<IResult> HandleCreateProductAsync(CreateProductRequest request, ISender sender)
    {
        try
        {
            // Adapt the CreateProductRequest object to a CreateProductCommand object using Mapster
            var command = request.Adapt<CreateProductCommand>();

            // Send the command to the Mediator and await the result
            var result = await sender.Send(command);

            // Adapt the result to a CreateProductResponse object using Mapster
            var response = result.Adapt<CreateProductResponse>();

            // Log the success and return a 201 Created status with the response and location of the new product
            _logger.LogInformation("Product '{ProductName}' created successfully with ID: {ProductId}", command.Name, response.Id);
            return Results.Created($"/products/{response.Id}", response);
        }
        catch (ValidationException ex)
        {
            // Log validation errors
            _logger.LogError(ex, "Validation failed for product creation request: {Request}", request);

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
            _logger.LogError(ex, "An unexpected error occurred while creating product: {Request}", request);

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
