namespace Ordering.Application.Data;

// IApplicationDbContext interface defines the contract for the database context
// This interface is part of the repository pattern used to abstract data access logic
public interface IApplicationDbContext
{
    // DbSet representing the collection of Customers in the database
    DbSet<Customer> Customers { get; }

    // DbSet representing the collection of Products in the database
    DbSet<Product> Products { get; }

    // DbSet representing the collection of Orders in the database
    DbSet<Order> Orders { get; }

    // DbSet representing the collection of OrderItems in the database
    DbSet<OrderItem> OrderItems { get; }

    // Method to save changes made in the context to the database
    // Takes a CancellationToken to handle cancellation of the save operation
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


/*
 * In a well-architected .NET application, having both an interface (IApplicationDbContext) in the (application project) 
 * and its implementation (ApplicationDbContext) in the (infrastructure project) is a common pattern. 
 * This pattern adheres to the principles of Clean Architecture and Separation of Concerns
 */