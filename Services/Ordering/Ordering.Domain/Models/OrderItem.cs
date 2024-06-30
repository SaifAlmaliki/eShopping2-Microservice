namespace Ordering.Domain.Models;
public class OrderItem : Entity<OrderItemId>
{
    // Internal constructor to initialize an 'OrderItem' instance.
    // This constructor is not accessible outside the assembly.
    internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    // Property to store the order ID associated with this order item.
    // The 'private set' modifier means it can only be set within this class.
    public OrderId OrderId { get; private set; } = default!;

    // Property to store the product ID associated with this order item.
    // The 'private set' modifier means it can only be set within this class.
    public ProductId ProductId { get; private set; } = default!;

    // Property to store the quantity of the product in the order item.
    // The 'private set' modifier means it can only be set within this class.
    public int Quantity { get; private set; } = default!;

    // Property to store the price of the product in the order item.
    // The 'private set' modifier means it can only be set within this class.
    public decimal Price { get; private set; } = default!;
}
