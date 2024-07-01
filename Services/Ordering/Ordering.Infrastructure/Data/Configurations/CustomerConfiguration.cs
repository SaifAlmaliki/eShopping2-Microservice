namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Set the primary key of the Customer entity
            builder.HasKey(c => c.Id);

            // Configure the Id property to use a custom conversion
            builder.Property(c => c.Id).HasConversion(
                // Convert CustomerId to a value that can be stored in the database
                customerId => customerId.Value,
                // Convert database value back to CustomerId
                dbId => CustomerId.Of(dbId)
            );

            // Set the maximum length of the Name property to 100 characters and make it required
            builder.Property(c => c.Name)
                   .HasMaxLength(100) // The Name field has a maximum length of 100 characters.
                   .IsRequired();              // The Name field is required (cannot be null).

            // Set the maximum length of the Email property to 255 characters
            builder.Property(c => c.Email)
                   .HasMaxLength(255); // The Email field has a maximum length of 255 characters.

            // Create a unique index on the Email property to ensure no duplicate emails
            builder.HasIndex(c => c.Email)
                   .IsUnique();       // Ensures the Email field has unique values.
        }
    }
}

// Summary of the CustomerConfiguration class:
// This class configures the entity properties and relationships for the Customer entity in the database.
// - Sets the primary key for the Customer entity.
// - Configures the ID property with custom conversion logic.
// - Sets up the Name property with a maximum length and required constraint.
// - Sets up the Email property with a maximum length.
// - Ensures the Email property is unique by creating a unique index.
