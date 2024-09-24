using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence.Interceptors;

/// <summary>
/// Responsible for auditing changes for updating the timestamps on entities
/// that implement the <see cref="ITrackable"/> interface.
/// </summary>
public sealed class AuditInterceptor : SaveChangesInterceptor
{
    /// <inheritdoc/>
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavedChanges(eventData, result);
    }

    /// <inheritdoc/>
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the timestamps of entities that implement the <see cref="ITrackable"/> interface.
    /// </summary>
    /// <param name="context">The DbContext instance where changes are being tracked.</param>
    private static void UpdateTimestamps(DbContext? context)
    {
        if (context == null) return;

        foreach(var entry in context.ChangeTracker.Entries())
        {
            var entityType = entry.GetType();

            if (!typeof(ITrackable).IsAssignableFrom(entityType)) return;

            UpdateEntryTimestampsByState(entry);
        }
    }

    /// <summary>
    /// Updates the CreatedAt and UpdatedAt fields based on the state of the entity.
    /// </summary>
    /// <param name="entry">The tracked entity entry.</param>
    private static void UpdateEntryTimestampsByState(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                var now = DateTimeOffset.UtcNow;
                entry.Property("CreatedAt").CurrentValue = now;
                entry.Property("UpdatedAt").CurrentValue = now;
                return;
            case EntityState.Modified:
                entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;
                return;
            default:
                return;
        }
    }
}
