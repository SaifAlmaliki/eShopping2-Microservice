namespace Basket.API.Extentions;
public static class ApplicationBuilderExtensions
{
    // Extension method for WebApplication to configure middleware
    public static void ConfigureMiddleware(this WebApplication app)
    {
        // Configure middleware for development environment
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable HTTPS redirection
        app.UseHttpsRedirection();

        // Map Carter endpoints for minimal APIs
        app.MapCarter();

        // Use custom exception handler middleware
        app.UseExceptionHandler(options => { });

        // Use HealthChecks middleware to enable health check endpoint
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            // Configure the response writer to use a custom response format
            // The ResponseWriter property is set to UIResponseWriter.WriteHealthCheckUIResponse
            // This method provides a user-friendly response format for health checks,
            // which can be useful for integrating with health check UI tools.
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}
