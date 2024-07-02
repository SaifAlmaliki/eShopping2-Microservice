using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
    (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        // Add services to the container.
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // Add any registered ISaveChangesInterceptor services to the DbContext options
            // This allows the application to use custom interceptors like AuditableEntityInterceptor
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            // Configure the DbContext to use SQL Server with the specified connection string
            options.UseSqlServer(connectionString);
        });

    //    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
