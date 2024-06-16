var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(); // Registers gRPC services with the dependency injection container
builder.Services.AddDbContext<DiscountContext>(opts =>
    // Configures the DbContext to use SQLite as the database provider
    opts.UseSqlite(builder.Configuration.GetConnectionString("Database"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Applies any pending migrations to the database during application startup
app.UseMigration();

// Maps the DiscountService to gRPC endpoints
app.MapGrpcService<DiscountService>();

app.MapGet("/", () =>
    // Maps a GET request to the root URL to return a message about gRPC communication
    "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
);

// Runs the application
app.Run();
