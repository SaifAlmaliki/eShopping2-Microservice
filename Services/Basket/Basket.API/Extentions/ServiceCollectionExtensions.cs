namespace Basket.API.Extentions;

public static class ServiceCollectionExtensions
{
    // Extension method for IServiceCollection to configure services
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        // Register Carter for minimal APIs
        services.AddCarter();

        // Add MediatR services and register custom behaviors for validation and logging
        services.AddMediatR(config =>
        {
            // Register services from the specified assembly
            config.RegisterServicesFromAssembly(assembly);
            // Add custom validation behavior
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // Add custom logging behavior
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        // Configure Marten for data services and set up the connection to PostgreSQL
        services.AddMarten(opts =>
        {
            opts.Connection(configuration.GetConnectionString("Database")!);

            // Configure the schema mapping for the ShoppingCart entity.
            // The Identity method is used to specify the property that will act as the unique identifier (Username).
            opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
        }).UseLightweightSessions();

        // Register the BasketRepository service in the dependency injection container.
        // This registers IBasketRepository with an implementation of BasketRepository.
        // AddScoped means that a new instance of BasketRepository will be created
        // for each client request (scope) to handle the basket operations.
        services.AddScoped<IBasketRepository, BasketRepository>();

        // Decorate the IBasketRepository service with CachedBasketRepository.
        // This means that CachedBasketRepository will wrap the original BasketRepository.
        // When IBasketRepository is requested, the DI container will inject CachedBasketRepository,
        // which will in turn use BasketRepository for the core operations, adding caching behavior.
        services.Decorate<IBasketRepository, CachedBasketRepository>();

       
        // AddStackExchangeRedisCache sets up Redis as the distributed cache for the application..
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        // Register and configure the gRPC client service for the DiscountProtoService
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            // Set the address for the gRPC service by reading it from the configuration settings
            options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            // Create a new HttpClientHandler to manage HTTP message handling
            var handler = new HttpClientHandler
            {
                // Set a custom validation callback to accept any server certificate
                // This is potentially dangerous as it skips SSL certificate validation
                // between basket and discount serrvices, making the application vulnerable
                // to man-in-the-middle attacks.
                // This should only be used in development environments or when you
                // have a specific reason to trust the server certificate.
                ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Return the configured HttpClientHandler (SSL verification skipped between services)
            return handler;
        });

        // Add asynchronous communication services using MassTransit with RabbitMQ
        services.AddMessageBroker(configuration);

        // Register the custom exception handler
        services.AddExceptionHandler<CustomExceptionHandler>();

        // Add Swagger for API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Configure health checks with PostgreSQL and Redis
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("Database")!)
            .AddRedis(configuration.GetConnectionString("Redis")!);

        // Return the service collection to allow chaining of service configuration
        return services;
    }
}

