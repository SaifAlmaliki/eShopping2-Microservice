namespace Ordering.Infrastructure.Data.Configurations;

// This class is used to configure the Product entity in the database.
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Set the primary key for the Product entity to the Id property.
        builder.HasKey(p => p.Id);

        // Configure the Id property to convert between the custom ProductId type and its underlying value type.
        builder.Property(p => p.Id).HasConversion(
            productId => productId.Value,        // Convert ProductId to its value type when saving to the database.
            dbId => ProductId.Of(dbId)    // Convert the value type back to ProductId when retrieving from the database.
        );

        // Configure the Name property for the Product entity.
        builder.Property(p => p.Name)
               .HasMaxLength(100) // The Name field has a maximum length of 100 characters.
               .IsRequired();     // The Name field is required (cannot be null).
    }
}

// Summary of the ProductConfiguration class:
// This class configures the entity properties for the Product entity in the database.
// - Sets the primary key and configures the ID conversion.
// - Configures the Name property with a maximum length and makes it required.
