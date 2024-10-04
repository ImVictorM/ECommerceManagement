using Domain.Common.Interfaces;

namespace Domain.Common.Models;

/// <summary>
/// Base class for domain entities.
/// An entity is characterize by having a unique identity.
/// Two entities are equal if they have the same identity.
/// </summary>
/// <typeparam name="TId">The type of the entity's unique identifier.</typeparam>
public abstract class Entity<TId> :
    ITrackable,
    IHasDomainEvent,
    IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>
    /// The list of events.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = [];
    /// <summary>
    /// Gets the entity's unique identifier.
    /// </summary>
    public TId Id { get; protected set; } = default!;
    /// <summary>
    /// Gets the list of domain events.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    /// <summary>
    /// Gets the date when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; protected set; }

    /// <summary>
    /// Gets the date when the entity was last updated.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier for this entity.</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && GetType() == entity.GetType() && Id.Equals(entity.Id);
    }

    /// <inheritdoc/>
    public virtual bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    /// <summary>
    /// Equality operator for entities.
    /// </summary>
    /// <param name="left">The left entity to compare.</param>
    /// <param name="right">The right entity to compare.</param>
    /// <returns>True if the left entity is equal to the right entity; otherwise, false.</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Inequality operator for entities.
    /// </summary>
    /// <param name="left">The left entity to compare.</param>
    /// <param name="right">The right entity to compare.</param>
    /// <returns>True if the left entity is not equal to the right entity; otherwise, false.</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc/>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
