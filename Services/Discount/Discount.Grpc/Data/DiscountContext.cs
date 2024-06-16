namespace Discount.Grpc.Data;

/* This file defines the DiscountContext class, which serves as the Entity Framework database context
 * for managing discount coupons. It configures the Coupons table and seeds initial data into the database, 
 * facilitating interaction with the discount data.
 */
public class DiscountContext : DbContext
{
    private readonly ILogger<DiscountContext> _logger;

    // Represents the Coupons table in the database.
    public DbSet<Coupon> Coupons { get; set; } = default!;

    // Constructor that takes DbContextOptions and ILogger, then passes options to the base class constructor.
    public DiscountContext(DbContextOptions<DiscountContext> options, ILogger<DiscountContext> logger) : base(options)
    {
        _logger = logger;
        _logger.LogInformation("DiscountContext initialized.");
    }

    // Method to configure the model (entity mappings) for the context.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogInformation("Configuring the model and seeding initial data.");

        // Seed initial data into the Coupons table.
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150 },
            new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 100 }
        );

        _logger.LogInformation("Model configured and initial data seeded.");
    }
}
