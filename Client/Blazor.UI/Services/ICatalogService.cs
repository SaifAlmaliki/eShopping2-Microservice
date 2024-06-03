namespace Blazor.UI.Services;

// Interface for catalog service, providing methods to fetch products
public interface ICatalogService
{
    // Fetches a paginated list of products.
    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

    // Fetches a product by its unique identifier.
    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);

    // Fetches products by their category.
    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);
}
