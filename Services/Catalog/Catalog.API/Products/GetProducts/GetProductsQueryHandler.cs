
namespace Catalog.API.Products.GetProducts;

// Define a query model to specify pagination parameters for retrieving products
public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

// Define a result model to return a collection of products
public record GetProductsResult(IEnumerable<Product> Products);

// Define a query handler to process the GetProductsQuery
internal class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    private readonly IDocumentSession _session;

    // Constructor to initialize the session and logger
    public GetProductsQueryHandler(IDocumentSession session)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    // Handle method to process the query and return the result
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;

        // Retrieve the list of products from the database
        var products = await _session.Query<Product>()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

        // Return the result with the list of products
        return new GetProductsResult(products);
    }
}
