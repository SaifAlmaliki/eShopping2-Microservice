using Catalog.API.Data;

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
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    
    // Get the current assembly reference.
    var assembly = typeof(Program).Assembly;

    // Add MediatR services from the assembly and register custom behaviors for validation and logging.
    builder.Services.AddMediatR(config =>
    {
        // Registers all MediatR handlers and related services from the specified assembly.
        config.RegisterServicesFromAssembly(assembly);

        // Registers the custom validation behavior to the MediatR pipeline.
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));

        // Registers the custom logging behavior to the MediatR pipeline.
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    });

    // Add FluentValidation services from the assembly
    // This automatically scans the assembly for Validator classes and registers them.
    builder.Services.AddValidatorsFromAssembly(assembly);

    // Add Carter for minimal API routing
    builder.Services.AddCarter();

    // Configure Marten with a connection string from the configuration and use lightweight sessions
    // Marten is a library for working with PostgreSQL as a document database and event store.
    builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    }).UseLightweightSessions();

    // Registers the CustomExceptionHandler to handle exceptions globally.
    builder.Services.AddExceptionHandler<CustomExceptionHandler>();

    // Configure health checks with PostgreSQL connection
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

    // Initialize Marten with CatalogInitialData during service configuration
    builder.Services.InitializeMartenWith<CatalogInitialData>();
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
    // Map Carter endpoints for minimal APIs
    app.MapCarter();

    // Use custom exception handler middleware
    // This will handle exceptions thrown during request processing.
    app.UseExceptionHandler(options => { });

    // Configure health check endpoint with a custom response writer
    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
}
