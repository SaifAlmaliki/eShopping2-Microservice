namespace Catalog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

// Internal class implementing IQueryHandler to handle GetProductByIdQuery
internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    // Constructor to inject the document session and logger dependencies
    public GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    // Method to handle the query and return the result
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", query.Id);

        // Load the product from the session using the provided ID
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);

        // Log the result of the load operation
        if (product is null)
        {
            _logger.LogWarning("Product with Id: {Id} was not found", query.Id);
            throw new ProductNotFoundException(query.Id);
        }
        else
        {
            _logger.LogInformation("Product with Id: {Id} was found", query.Id);
        }

        // Return the result with the loaded product
        return new GetProductByIdResult(product);
    }
}
