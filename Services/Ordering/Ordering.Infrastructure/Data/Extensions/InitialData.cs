namespace Ordering.Infrastructure.Data.Extensions;
public class InitialData
{
    // Static property to get a list of customers
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(
                CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                "Mehmet",
                "mehmet@gmail.com"),
            Customer.Create(
                CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")),
                "John",
                "john@gmail.com")
        };

    // Static property to get a list of products
    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(
                ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")),
                "IPhone X",
                500),
            Product.Create(
                ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")),
                "Samsung 10",
                400),
            Product.Create(
                ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")),
                "Huawei Plus",
                650),
            Product.Create(
                ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")),
                "Xiaomi Mi",
                450)
        };

    // Static property to get a list of orders with items
    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            // Creating addresses for customers
            var address1 = Address.Of("Mehmet", "Ozkaya", "mehmet@gmail.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
            var address2 = Address.Of("John", "Doe", "john@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

            // Creating payment information for customers
            var payment1 = Payment.Of("Mehmet", "5555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("John", "8885555555554444", "06/30", "222", 2);

            // Creating orders and adding items to them
            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                OrderName.Of("ORD_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment: payment1);
            order1.Add(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 2, 500);
            order1.Add(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 400);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("189dc8dc-990f-48e0-a37b-e6f2b60b9d7d")),
                OrderName.Of("ORD_2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment: payment2);
            order2.Add(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 650);
            order2.Add(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 450);

            // Returning the list of orders with items
            return new List<Order> { order1, order2 };
        }
    }
}

/* 
Short Description:
This file defines the InitialData class, which provides initial data for customers, products, and orders in the application. 
The Customers property returns a list of predefined customers. 
The Products property returns a list of predefined products. 
The OrdersWithItems property returns a list of predefined orders, each containing items and associated customer information, addresses, and payment details. 
This initial data can be used for seeding a database or for testing purposes.
*/
