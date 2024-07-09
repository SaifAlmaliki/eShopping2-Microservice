using Basket.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Configure services by calling the extension method to keep the code modular and clean
builder.Services.ConfigureServices(builder.Configuration, typeof(Program).Assembly);

var app = builder.Build();

// Configure middleware by calling the extension method
app.ConfigureMiddleware();

// Run the application
app.Run();
