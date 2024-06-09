namespace Catalog.API.Products.GetProductByCategory;

// Record to represent the query with a single property 'Category'
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

// Record to represent the result containing a collection of products
public record GetProductByCategoryResult(IEnumerable<Product> Products);

// Handler class for processing the GetProductByCategoryQuery
public class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;

    // Constructor with dependency injection
    public GetProductByCategoryQueryHandler(IDocumentSession session)
    {
        _session = session;
    }

    // Method to handle the query
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        // Fetch products that match the category criteria
        var products = await _session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}