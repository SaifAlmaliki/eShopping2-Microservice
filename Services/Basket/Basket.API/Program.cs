var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder);

var app = builder.Build();

// Configure middleware
ConfigureMiddleware(app);

// Run the application
app.Run();

void ConfigureServices(WebApplicationBuilder builder)
{
    // Application Services.
    var assembly = typeof(Program).Assembly;
    builder.Services.AddCarter();

    // Add MediatR services from the assembly and register custom behaviors for validation and logging.
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    // Data Services
    builder.Services.AddMarten(opts =>
    {
        // Set the connection string for Marten to connect to PostgreSQL.
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);

        // Configure the schema mapping for the ShoppingCart entity.
        // The Identity method is used to specify the property that will act as the unique identifier.
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();


    // Register the BasketRepository service in the dependency injection container.
    // This registers IBasketRepository with an implementation of BasketRepository.
    // AddScoped means that a new instance of BasketRepository will be created
    // for each client request (scope) to handle the basket operations.
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    // Decorate the IBasketRepository service with CachedBasketRepository.
    // This means that CachedBasketRepository will wrap the original BasketRepository.
    // When IBasketRepository is requested, the DI container will inject CachedBasketRepository,
    // which will in turn use BasketRepository for the core operations, adding caching behavior.
    builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

    // Configure Redis caching in the dependency injection container.
    // AddStackExchangeRedisCache sets up Redis as the distributed cache for the application..
    // The Configuration property is set to the connection string for Redis, which is retrieved
    // from the application's configuration (e.g., appsettings.json or environment variables).
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    // Register and configure the gRPC client service for the DiscountProtoService
    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
    {
        // Set the address for the gRPC service by reading it from the configuration settings
        options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
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



    // Register the custom exception handler
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    // Add Swagger for API documentation
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configure health checks with PostgreSQL and Redis
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
}

void ConfigureMiddleware(WebApplication app)
{
    // Configure middleware for development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Configure the HTTP request pipeline
    app.UseHttpsRedirection();

    // Map Carter endpoints for minimal APIs
    app.MapCarter();

    // Use custom exception handler middleware
    app.UseExceptionHandler(options => { });

    // UseHealthChecks middleware to enable health check endpoint
    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            // Configure the response writer to use a custom response format
            // The ResponseWriter property is set to UIResponseWriter.WriteHealthCheckUIResponse
            // This method provides a user-friendly response format for health checks,
            // which can be useful for integrating with health check UI tools.
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

}
