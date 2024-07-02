namespace Ordering.Infrastructure.Data.Extensions;

// Static class to provide database-related extensions
public static class DatabaseExtensions
{
    // Extension method to initialize the database asynchronously
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        // Create a scope for service provider
        using var scope = app.Services.CreateScope();

        // Get the database context from the service provider
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply pending migrations to the database
        await context.Database.MigrateAsync();

        // Seed the database with initial data
        await SeedAsync(context);
    }

    // Private method to seed the database with initial data
    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        await SeedOrdersWithItemsAsync(context);
    }

    // Private method to seed customers if the Customers table is empty
    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    // Private method to seed products if the Products table is empty
    private static async Task SeedProductAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    // Private method to seed orders with items if the Orders table is empty
    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await context.SaveChangesAsync();
        }
    }
}

/* 
Short Description:
This file defines the DatabaseExtensions static class, which provides extension methods for initializing and seeding the database. 
The InitialiseDatabaseAsync method: applies pending migrations and seeds the database with initial data. 
The SeedAsync method: coordinates the seeding process by calling specific methods to seed customers, products, and orders with items if their respective tables are empty. 
These methods ensure that the database is populated with essential initial data, which is useful for setting up a new environment or for testing purposes.
*/
