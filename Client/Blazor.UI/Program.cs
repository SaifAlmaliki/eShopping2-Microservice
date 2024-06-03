var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()   // Add Razor Components service.
    .AddInteractiveServerComponents();  // Add support for interactive server-side components.

// Add a Refit client for the catalog service
builder.Services.AddRefitClient<ICatalogService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

// Add a Refit client for the basket service.
builder.Services.AddRefitClient<IBasketService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

// Add a Refit client for the ordering service.
builder.Services.AddRefitClient<IOrderingService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]!);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS.
app.UseStaticFiles();       // Serve static files.
app.UseRouting();           // Enable routing.
app.UseAuthorization();     // Enable authorization.
app.UseAntiforgery();       // Use antiforgery token generation and validation.

app.MapRazorComponents<App>()          // Map the Razor components to the application.
    .AddInteractiveServerRenderMode(); // Enable interactive server-side rendering mode.

app.Run();
