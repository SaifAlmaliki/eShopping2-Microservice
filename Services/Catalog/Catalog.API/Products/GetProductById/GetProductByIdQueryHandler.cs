namespace Catalog.API.Products.GetProductById;
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

// Internal class implementing IQueryHandler to handle GetProductByIdQuery
internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session;

    // Constructor to inject the document session and logger dependencies
    public GetProductByIdQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    // Method to handle the query and return the result
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        // Load the product from the session using the provided ID
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);

        // Log the result of the load operation
        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        // Return the result with the loaded product
        return new GetProductByIdResult(product);
    }
}
