using SharedKernel.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Common.Persistence.Interceptors;

/// <summary>
/// Intercepts save operations to automatically update timestamp properties
/// for entities implementing the <see cref="IAuditable"/> interface.
/// </summary>
public sealed class AuditInterceptor : SaveChangesInterceptor
{
    /// <inheritdoc/>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    /// <inheritdoc/>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries())
        {
            var entityType = entry.Entity.GetType();

            if (!typeof(IAuditable).IsAssignableFrom(entityType))
            {
                return;
            }

            UpdateEntryTimestampsByState(entry);
        }
    }

    private static void UpdateEntryTimestampsByState(EntityEntry entry)
    {
        switch (entry.State)
        {
            case EntityState.Added:
                var now = DateTimeOffset.UtcNow;
                entry.Property(nameof(IAuditable.CreatedAt)).CurrentValue = now;
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
                return;
            case EntityState.Modified:
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = DateTimeOffset.UtcNow;
                return;
            default:
                return;
        }
    }
}
