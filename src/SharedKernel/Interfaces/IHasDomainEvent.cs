namespace SharedKernel.Interfaces;

/// <summary>
/// Contract for handling domain events.
/// </summary>
public interface IHasDomainEvent
{
    /// <summary>
    /// Gets the list of domain events.
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clear the list of domain events.
    /// </summary>
    void ClearDomainEvents();

    /// <summary>
    /// Adds a new domain event to the domain event list.
    /// </summary>
    /// <param name="domainEvent">The domain event to be added.</param>
    void AddDomainEvent(IDomainEvent domainEvent);
}
