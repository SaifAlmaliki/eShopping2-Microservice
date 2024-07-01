namespace Ordering.Infrastructure.Data.Configurations
{
    // This class is used to configure the OrderItem entity in the database.
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Set the primary key for the OrderItem entity to the Id property.
            builder.HasKey(oi => oi.Id);

            // Configure the Id property to convert between the custom OrderItemId type and its underlying value type.
            builder.Property(oi => oi.Id).HasConversion(
                orderItemId => orderItemId.Value, // Convert OrderItemId to its value type when saving to the database.
                dbId => OrderItemId.Of(dbId)      // Convert the value type back to OrderItemId when retrieving from the database.
            );

            // Set up a relationship to the Product entity.
            // This means each OrderItem is associated with a Product.
            builder.HasOne<Product>()
                   .WithMany()                   // A Product can have many OrderItems.
                   .HasForeignKey(oi => oi.ProductId); // The foreign key property in OrderItem is ProductId.

            // Configure the Quantity property for the OrderItem entity.
            builder.Property(oi => oi.Quantity)
                   .IsRequired();                // The Quantity field is required (cannot be null).

            // Configure the Price property for the OrderItem entity.
            builder.Property(oi => oi.Price)
                   .IsRequired();                // The Price field is required (cannot be null).
        }
    }
}

// Summary of the OrderItemConfiguration class:
// This class configures the entity properties and relationships for the OrderItem entity in the database.
// - Sets the primary key and configures the ID conversion.
// - Configures the foreign key relationship with the Product entity.
// - Configures the Quantity property and makes it required.
// - Config
