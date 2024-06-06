namespace Catalog.API.Products.CreateProduct;

// Record type representing the request to create a product
public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

// Record type representing the response after a product is created
public record CreateProductResponse(Guid Id);

// Class implementing the ICarterModule interface to define endpoint routes
public class CreateProductEndPoint : ICarterModule
{
    // Method to add routes to the endpoint route builder
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Define a POST endpoint at "/products"
        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
            {
                // Adapt the CreateProductRequest object to a CreateProductCommand object using Mapster
                var command = request.Adapt<CreateProductCommand>();

                // Send the command to the Mediator and await the result
                var result = await sender.Send(command);

                // Adapt the result to a CreateProductResponse object using Mapster
                var response = result.Adapt<CreateProductResponse>();

                // Return a 201 Created status with the response and location of the new product
                return Results.Created($"/products/{response.Id}", response);
            })
            // Set the name of the endpoint to "CreateProduct"
            .WithName("CreateProduct")
            // Specify that the endpoint can produce a CreateProductResponse with a 201 status code
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            // Specify that the endpoint can produce a problem response with a 400 status code
            .ProducesProblem(StatusCodes.Status400BadRequest)
            // Provide a summary for the endpoint
            .WithSummary("Create Product")
            // Provide a description for the endpoint
            .WithDescription("Create Product");
    }
}