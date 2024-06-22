namespace Ordering.Application;

// Define a static class to contain extension methods for dependency injection
public static class DependencyInjection
{
    // Define an extension method for IServiceCollection to add application-specific services
    public static IServiceCollection AddApplicationService
    (this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR services to the service collection
        services.AddMediatR(config =>
        {
            // Register all MediatR services from the assembly containing this code
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // Add the ValidationBehavior to the MediatR pipeline
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // Add the LoggingBehavior to the MediatR pipeline
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Add feature management services to the service collection
        services.AddFeatureManagement();

        // Uncomment the following line to add message broker services
        // services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        // Return the updated service collection
        return services;
    }
}


/* The syntax <,> in typeof(ValidationBehavior<,>) and typeof(LoggingBehavior<,>) is used 
 * to refer to open generic types. This is necessary when you want to register behaviors
 * that can handle multiple different types of requests and responses generically in MediatR
 * 
 * For example, you might define a generic class ValidationBehavior<TRequest, TResponse> that
 * can validate different types of requests and produce different types of responses.
 */ 