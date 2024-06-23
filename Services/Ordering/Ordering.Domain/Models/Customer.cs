namespace Ordering.Domain.Models;

public class Customer: Entity<CustomerId>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };

        return customer;
    }
}
