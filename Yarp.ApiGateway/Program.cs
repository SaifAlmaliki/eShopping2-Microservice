using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add the YARP reverse proxy services to the dependency injection container
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add rate limiting services to the container
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    // Define a fixed window rate limiter named "fixed"
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        // Set the time window for rate limiting to 10 seconds
        options.Window = TimeSpan.FromSeconds(10);
        // Set the maximum number of requests allowed within the time window to 5
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Add the rate limiter middleware to the request pipeline
app.UseRateLimiter();

// Map reverse proxy routes to the request pipeline
app.MapReverseProxy();

// Run the application
app.Run();
