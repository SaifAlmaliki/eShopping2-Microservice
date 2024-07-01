namespace Ordering.Infrastructure.Data.Configurations;

// This class is used to configure the Order entity in the database.
// It implements the IEntityTypeConfiguration interface for the Order type.
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Set the primary key for the Order entity to the Id property.
        builder.HasKey(o => o.Id);

        // Configure the Id property to convert between the custom OrderId type and its underlying value type.
        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,        // Convert OrderId to its value type when saving to the database.
            dbId => OrderId.Of(dbId)  // Convert the value type back to OrderId when retrieving from the database.
        );

        // Set up a one-one relationship to the Customer entity.
        // This means each Order must be associated with a Customer.
        builder.HasOne<Customer>()
               .WithMany()              // A Customer can have many Orders.
               .HasForeignKey(o => o.CustomerId) // The foreign key property in Order is CustomerId.
               .IsRequired();           // This relationship is mandatory.

        // Set up a one-to-many relationship with the OrderItems entity.
        // An Order can have many OrderItems.
        builder.HasMany(o => o.OrderItems)
               .WithOne()               // Each OrderItem is associated with one Order.
               .HasForeignKey(oi => oi.OrderId); // The foreign key property in OrderItem is OrderId.

        // Configure the OrderName property, which is a complex property.
        // Complex properties are not simple scalar types and may require additional configuration.
        builder.ComplexProperty(o => o.OrderName, nameBuilder =>
        {
            // Set the column name, max length, and make it required.
            nameBuilder.Property(n => n.Value)
                       .HasColumnName(nameof(Order.OrderName))
                       .HasMaxLength(100)
                       .IsRequired();
        });

        // Configure the ShippingAddress property, which is another complex property.
        // Each part of the ShippingAddress needs to be configured individually.
        builder.ComplexProperty(
           o => o.ShippingAddress, addressBuilder =>
           {
               addressBuilder.Property(a => a.FirstName)
                   .HasMaxLength(50)
                   .IsRequired(); // The FirstName field is required and has a max length of 50.

               addressBuilder.Property(a => a.LastName)
                   .HasMaxLength(50)
                   .IsRequired(); // The LastName field is required and has a max length of 50.

               addressBuilder.Property(a => a.EmailAddress)
                   .HasMaxLength(50); // The EmailAddress field has a max length of 50.

               addressBuilder.Property(a => a.AddressLine)
                   .HasMaxLength(180)
                   .IsRequired(); // The AddressLine field is required and has a max length of 180.

               addressBuilder.Property(a => a.Country)
                   .HasMaxLength(50); // The Country field has a max length of 50.

               addressBuilder.Property(a => a.State)
                   .HasMaxLength(50); // The State field has a max length of 50.

               addressBuilder.Property(a => a.ZipCode)
                   .HasMaxLength(5)
                   .IsRequired(); // The ZipCode field is required and has a max length of 5.
           });

        // Configure the BillingAddress property similarly to ShippingAddress.
        builder.ComplexProperty(
          o => o.BillingAddress, addressBuilder =>
          {
              addressBuilder.Property(a => a.FirstName)
                   .HasMaxLength(50)
                   .IsRequired();

              addressBuilder.Property(a => a.LastName)
                   .HasMaxLength(50)
                   .IsRequired();

              addressBuilder.Property(a => a.EmailAddress)
                  .HasMaxLength(50);

              addressBuilder.Property(a => a.AddressLine)
                  .HasMaxLength(180)
                  .IsRequired();

              addressBuilder.Property(a => a.Country)
                  .HasMaxLength(50);

              addressBuilder.Property(a => a.State)
                  .HasMaxLength(50);

              addressBuilder.Property(a => a.ZipCode)
                  .HasMaxLength(5)
                  .IsRequired();
          });

        // Configure the Payment property, another complex property.
        builder.ComplexProperty(
               o => o.Payment, paymentBuilder =>
               {
                   paymentBuilder.Property(p => p.CardName)
                       .HasMaxLength(50); // The CardName field has a max length of 50.

                   paymentBuilder.Property(p => p.CardNumber)
                       .HasMaxLength(24)
                       .IsRequired(); // The CardNumber field is required and has a max length of 24.

                   paymentBuilder.Property(p => p.Expiration)
                       .HasMaxLength(10); // The Expiration field has a max length of 10.

                   paymentBuilder.Property(p => p.CVV)
                       .HasMaxLength(3); // The CVV field has a max length of 3.

                   paymentBuilder.Property(p => p.PaymentMethod); // The PaymentMethod field.
               });

        // Configure the Status property, setting a default value and conversion logic.
        builder.Property(o => o.Status)
               .HasDefaultValue(OrderStatus.Draft)     // Default value is set to Draft.
               .HasConversion(
                   s => s.ToString(), // Convert the OrderStatus enum to string when saving to the database.
                   dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus) // Convert string back to OrderStatus enum when retrieving from the database.
               );

        // Configure the TotalPrice property.
        builder.Property(o => o.TotalPrice); // No additional configuration needed for this property.
    }
}

// Summary of the OrderConfiguration class:
// This class configures the entity properties and relationships for the Order entity in the database.
// - Sets the primary key and configures the ID conversion.
// - Configures the foreign key relationship with the Customer entity.
// - Configures the one-to-many relationship with OrderItems.
// - Configures complex properties for OrderName, ShippingAddress, BillingAddress, and Payment.
// - Sets default value and conversion for the Status property.
// - Defines properties for the entity including TotalPrice.
