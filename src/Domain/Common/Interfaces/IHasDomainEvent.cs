namespace Domain.Common.Interfaces;

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
}
