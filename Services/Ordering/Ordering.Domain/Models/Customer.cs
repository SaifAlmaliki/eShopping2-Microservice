namespace Ordering.Domain.Models;
public class Customer : Entity<CustomerId>
{
    // Property to store the name of the customer.
    public string Name { get; set; }

    // Property to store the email address of the customer.
    public string Email { get; set; }

    // Static method to create a new 'Customer' instance.
    // It takes an ID, name, and email as parameters.
    public static Customer Create(CustomerId id, string name, string email)
    {
        // Throws an exception if the 'name' parameter is null or whitespace.
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        // Throws an exception if the 'email' parameter is null or whitespace.
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        // Creates a new 'Customer' instance and sets its properties.
        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };

        // Returns the newly created 'Customer' instance.
        return customer;
    }
}
