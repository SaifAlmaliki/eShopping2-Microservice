namespace Ordering.Infrastructure.Data;

// ApplicationDbContext provides the implementation for the database context
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    // Constructor that takes DbContextOptions and passes it to the base DbContext class
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Define a DbSet for Customer entities, allowing CRUD operations on Customers
    public DbSet<Customer> Customers => Set<Customer>();

    // Define a DbSet for Product entities, allowing CRUD operations on Products
    public DbSet<Product> Products => Set<Product>();

    // Define a DbSet for Order entities, allowing CRUD operations on Orders
    public DbSet<Order> Orders => Set<Order>();

    // Define a DbSet for OrderItem entities, allowing CRUD operations on OrderItems
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    // Override the OnModelCreating method to configure the model using the ModelBuilder
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Apply all configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Call the base class's OnModelCreating method to ensure any additional configurations are applied
        base.OnModelCreating(builder);
    }
}


/* 
 * A DbSet: represents a collection of entities of a specific type that can be queried from the database and updated. 
 * Set<Customer>() is a method provided by DbContext to get a DbSet for the specified entity type
 */

/*
 * In a well-architected .NET application, having both an interface (IApplicationDbContext) in the (application project) 
 * and its implementation (ApplicationDbContext) in the (infrastructure project) is a common pattern. 
 * This pattern adheres to the principles of Clean Architecture and Separation of Concerns
 */