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
    // Add services to the container.
    var assembly = typeof(Program).Assembly;

    // Add Carter for minimal API routing
    builder.Services.AddCarter();

    // Add MediatR services from the assembly and register custom behaviors for validation and logging.
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(assembly);
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    builder.Services.AddMarten(opts =>
    {
        // Set the connection string for Marten to connect to PostgreSQL.
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);

        // Configure the schema mapping for the ShoppingCart entity.
        // The Identity method is used to specify the property that will act as the unique identifier.
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    }).UseLightweightSessions();


    // Register the BasketRepository service
    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    // Configure Redis caching
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
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

    // Configure health check endpoint with a custom response writer
    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
}
