namespace Ordering.Infrastructure.Data.Interceptors;

// Interceptor to handle auditable entities, setting CreatedBy, CreatedAt, LastModifiedBy, and LastModified properties
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    // Override for synchronous saving changes
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context); // Update entities before saving changes
        return base.SavingChanges(eventData, result); // Call the base method to continue saving
    }

    // Override for asynchronous saving changes
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context); // Update entities before saving changes
        return base.SavingChangesAsync(eventData, result, cancellationToken); // Call the base method to continue saving
    }

    // Method to update the audit fields of entities
    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return; // If context is null, return immediately

        // Iterate through all tracked entities implementing IEntity
        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            // If entity is in Added state, set CreatedBy and CreatedAt properties
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "mehmet";        // Replace with dynamic user if necessary
                entry.Entity.CreatedAt = DateTime.UtcNow; // Set to current UTC time
            }

            // If entity is in Added or Modified state, or has changed owned entities, set LastModifiedBy and LastModified properties
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "mehmet";      // Replace with dynamic user if necessary
                entry.Entity.LastModified = DateTime.UtcNow; // Set to current UTC time
            }
        }
    }
}

// Extension methods for EntityEntry
public static class Extensions
{
    // Check if any owned entities have been added or modified
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null && // Ensure target entry is not null
            r.TargetEntry.Metadata.IsOwned() && // Ensure target entry is owned
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)); // Check if state is Added or Modified
}

/*
Short Description:
The AuditableEntityInterceptor class is a custom SaveChangesInterceptor that automatically sets auditing properties 
(CreatedBy, CreatedAt, LastModifiedBy, LastModified) on entities implementing the IEntity interface during the SaveChanges process. 
This ensures that these properties are correctly updated whenever entities are added or modified in the DbContext. 
The Extensions class adds an extension method to EntityEntry to check if any owned entities have changed, supporting the auditing process.
*/
