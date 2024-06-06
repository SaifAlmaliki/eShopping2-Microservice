// The purpose of this code is to handle the query for retrieving a paginated list of products from the catalog system.
// It defines the query and result models, as well as the handler to process the query and return the list of products.
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

// Define a query model to specify pagination parameters for retrieving products
public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

// Define a result model to return a collection of products
public record GetProductsResult(IEnumerable<Product> Products);

// Define a query handler to process the GetProductsQuery
internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    // Handle method to process the query and return the result
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        // Retrieve the list of products from the database
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        // Return the result with the list of products
        return new GetProductsResult(products);
    }
}
