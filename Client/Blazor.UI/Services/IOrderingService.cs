namespace Blazor.UI.Services;

// This interface, IOrderingService, specifies methods for fetching order data from an ordering service API.
public interface IOrderingService
{
    // Fetch paginated list of orders
    [Get("/ordering-service/orders?pageIndex={pageIndex}&pageSize={pageSize}\"")]
    Task<GetOrdersResponse> GetOrders(int? pageIndex = 1, int? pageSize = 10);

    // Fetch orders by order name
    [Get("/ordering-service/orders/{orderName}")]
    Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);

    // Fetch orders by customer ID
    [Get("/ordering-service/orders/customer/{customerId}")]
    Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);
}
