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
        // Creates a scope for the application's services.
        using var scope = app.ApplicationServices.CreateScope();

        // Resolves the ILogger<DiscountContext> service from the service provider for logging.
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DiscountContext>>();

        try
        {
            // Logs the start of the migration process.
            logger.LogInformation("Starting database migration.");

            // Resolves the DiscountContext service from the service provider.
            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();

            // Applies any pending migrations for the context to the database asynchronously.
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
