namespace Discount.Grpc.Data;

/* The purpose of this file is to provide an extension method that simplifies the process of 
 * applying database migrations during the application startup. By using the UseMigration method, 
 * the application ensures that the database schema is always up-to-date with the latest model changes. 
 * Rather than writing the update-database command manually, it automates the migration process and reduces
 * the risk of manual errors.
 */

// This static class contains extension methods for the application.
public static class Extentions
{
    // This method applies any pending migrations for the context to the database.
    // It ensures that the database schema is up-to-date.
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        // Creates a new scope for the application's services.
        // This scope is used to resolve scoped services from the application's dependency injection container.
        // The use of a scope ensures that any services instantiated within this context are properly disposed of
        // when the scope is closed, maintaining good resource management and preventing memory leaks.
        using var scope = app.ApplicationServices.CreateScope();

        // Resolves the ILogger<DiscountContext> service from the service provider for logging.
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DiscountContext>>();

        try
        {
            // Logs the start of the migration process.
            logger.LogInformation("Starting database migration.");

            // Gets the DiscountContext service from the service provider.
            // By using 'using var', we ensure that this tool is cleaned up properly after we're done using it.
            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            // Applies any pending migrations for the context to the database asynchronously.
            // ".MigrateAsync()" means it runs in the background without stopping the rest of the program. 
            // ".Wait()" is used here to make sure this process finishes before moving on to the next step.
            dbContext.Database.MigrateAsync().Wait();

            // Logs the successful completion of the migration process.
            logger.LogInformation("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            // Logs any exceptions that occur during the migration process.
            logger.LogError(ex, "An error occurred while migrating the database.");
        }

        // Returns the application builder to allow for fluent chaining.
        return app;
    }
}
