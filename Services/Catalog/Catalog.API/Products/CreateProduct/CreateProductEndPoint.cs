namespace Catalog.API.Products.CreateProduct;
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);

// Class implementing the ICarterModule interface to define endpoint routes
public class CreateProductEndPoint : ICarterModule
{
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
        // Adapt the CreateProductRequest object to a CreateProductCommand object using Mapster
        var command = request.Adapt<CreateProductCommand>();

        // Send the command to the Mediator and await the result
        var result = await sender.Send(command);

        // Adapt the result to a CreateProductResponse object using Mapster
        var response = result.Adapt<CreateProductResponse>();

        // return a 201 Created status with the response and location of the new product
        return Results.Created($"/products/{response.Id}", response);
    }
}
