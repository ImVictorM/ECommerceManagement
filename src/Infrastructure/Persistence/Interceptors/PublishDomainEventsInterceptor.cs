using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Interfaces;

namespace Infrastructure.Persistence.Interceptors;

/// <summary>
/// Interceptor used to publish domain events after changes were save.
/// Use eventual consistency or transactions to maintain data integrity.
/// </summary>
public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    /// <summary>
    /// Initiates a new instance of the <see cref="PublishDomainEventsInterceptor"/> class.
    /// </summary>
    /// <param name="publisher">The publisher of the event.</param>
    public PublishDomainEventsInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    /// <inheritdoc/>
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();

        return base.SavedChanges(eventData, result);
    }
 
    /// <inheritdoc/>
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext == null)
        {
            return;
        }

        var entitiesWithDomainEvents = dbContext.ChangeTracker
            .Entries<IHasDomainEvent>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithDomainEvents.SelectMany(e => e.DomainEvents).ToList();

        entitiesWithDomainEvents.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}
