using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom application, infrastructure, and API services to the container
builder.Services
    .AddApplicationService(builder.Configuration) // Custom method to add application services
    .AddInfrastructureServices(builder.Configuration) // Custom method to add infrastructure services
    .AddApiServices(builder.Configuration); // Custom method to add API services

var app = builder.Build();

// Configure API services for the application
app.UseApiServices();

// Configure the HTTP request pipeline for development environment
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync(); // Initialize, Migrate and seed the database
    app.UseSwagger();   // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwaggerUI(); // Enable middleware to serve Swagger UI
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Run the application
app.Run();
