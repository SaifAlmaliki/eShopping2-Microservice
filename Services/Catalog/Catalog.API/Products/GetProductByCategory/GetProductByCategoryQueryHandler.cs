namespace Catalog.API.Products.GetProductByCategory;

// Record to represent the query with a single property 'Category'
public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

// Record to represent the result containing a collection of products
public record GetProductByCategoryResult(IEnumerable<Product> Products);

// Handler class for processing the GetProductByCategoryQuery
public class GetProductByCategoryQueryHandler : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByCategoryQueryHandler> _logger;

    // Constructor with dependency injection
    public GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    // Method to handle the query
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByCategoryQuery for Category: {Category}", query.Category);

        try
        {
            // Fetch products that match the category criteria
            var products = await _session.Query<Product>()
                .Where(p => p.Category.Contains(query.Category))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("{ProductCount} products found for Category: {Category}", products.Count, query.Category);

            return new GetProductByCategoryResult(products);
        }
        catch (Exception ex)
        {
            // Log any exceptions that occur during query processing
            _logger.LogError(ex, "Error occurred while handling GetProductByCategoryQuery for Category: {Category}", query.Category);
            throw;
        }
    }
}