// The purpose of this code is to handle the query for retrieving a paginated list of products from the catalog system.
// It defines the query and result models, as well as the handler to process the query and return the list of products.
namespace Catalog.API.Products.GetProducts;

// Define a query model to specify pagination parameters for retrieving products
public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

// Define a result model to return a collection of products
public record GetProductsResult(IEnumerable<Product> Products);

// Define a query handler to process the GetProductsQuery
internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    // Constructor to initialize the session and logger
    public GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // Handle method to process the query and return the result
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;

        _logger.LogDebug("Retrieving products - Page Number: {PageNumber}, Page Size: {PageSize}", pageNumber, pageSize);

        // Retrieve the list of products from the database
        var products = await _session.Query<Product>()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

        _logger.LogInformation("Retrieved {ProductCount} products", products.Count);

        // Return the result with the list of products
        return new GetProductsResult(products);
    }
}
