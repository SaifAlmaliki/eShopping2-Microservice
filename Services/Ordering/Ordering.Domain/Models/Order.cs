namespace Ordering.Domain.Models;

public class Order : Aggregate<OrderId>
{
    // A private list to store the order items.
    private readonly List<OrderItem> _orderItems = new();

    // A public property to provide read-only access to the order items.
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public CustomerId CustomerId { get; private set; } = default!;
    public OrderName OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public Address BillingAddress { get; private set; } = default!;
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    // Property to calculate and get the total price of the order.
    // 'private set' is intentionally left empty to prevent setting it directly.
    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }

    // Static method to create a new 'Order' instance.
    public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
    {
        // Creates a new 'Order' instance and sets its properties.
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Payment = payment,
            Status = OrderStatus.Pending
        };

        // Adds a domain event indicating the order was created.
        order.AddDomainEvent(new OrderCreatedEvent(order));

        // Returns the newly created 'Order' instance.
        return order;
    }

    // Method to update the properties of the order.
    public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment, OrderStatus status)
    {
        // Sets the new values for the order properties.
        OrderName = orderName;
        ShippingAddress = shippingAddress;
        BillingAddress = billingAddress;
        Payment = payment;
        Status = status;

        // Adds a domain event indicating the order was updated.
        AddDomainEvent(new OrderUpdatedEvent(this));
    }

    // Method to add an item to the order.
    public void Add(ProductId productId, int quantity, decimal price)
    {
        // Throws an exception if quantity or price is negative or zero.
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        // Creates a new 'OrderItem' and adds it to the order items list.
        var orderItem = new OrderItem(Id, productId, quantity, price);
        _orderItems.Add(orderItem);
    }

    // Method to remove an item from the order.
    public void Remove(ProductId productId)
    {
        // Finds the order item with the specified product ID.
        var orderItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);

        // If the order item is found, removes it from the order items list.
        if (orderItem is not null)
        {
            _orderItems.Remove(orderItem);
        }
    }
}


/* The purpose of the Order class in the provided code is to represent an order in the domain of an ordering system. 
 * It serves as an aggregate root, which is a central concept in domain-driven design. 
 * As an aggregate root, the Order class encapsulates and manages the entire lifecycle of 
 * the order and its related entities, ensuring consistency and enforcing business rules.
 */