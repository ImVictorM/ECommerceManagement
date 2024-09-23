namespace Domain.Common.Models;

/// <summary>
/// Base class for domain aggregate roots.
/// An aggregate root is a special type of entity that servers
/// as the entry point to an aggregate.
/// It controls access to all other entities within the aggregate.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the aggregate root.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class with the specified unique identifier
    /// </summary>
    /// <param name="id">The unique identifier for this aggregate root.</param>
    protected AggregateRoot(TId id) : base(id) { }

    protected AggregateRoot() { }
}
