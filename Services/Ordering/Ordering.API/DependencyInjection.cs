namespace Ordering.API;

public static class DependencyInjection
{
    // Extension method to add services to the IServiceCollection
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adds Carter services to the IServiceCollection
        services.AddCarter();

        // Adds a custom exception handler to the IServiceCollection
        services.AddExceptionHandler<CustomExceptionHandler>();

        // Adds health check services to the IServiceCollection
        // Configures the health checks to use SQL Server with the provided connection string
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database")!);

        return services; // Returns the modified IServiceCollection
    }

    // Extension method to configure the middleware pipeline of the WebApplication
    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Maps Carter endpoints in the WebApplication
        app.MapCarter();

        // Configures the application to use a custom exception handler middleware
        app.UseExceptionHandler(options => { });

        // Configures health check endpoint with a custom response writer
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

        return app; // Returns the modified WebApplication
    }
}
