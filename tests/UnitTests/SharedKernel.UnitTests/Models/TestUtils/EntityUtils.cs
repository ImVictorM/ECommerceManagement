using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.UnitTests.Models.TestUtils;

/// <summary>
/// Utilities for the <see cref="Entity{TId}"/> model.
/// </summary>
public static class EntityUtils
{
    /// <summary>
    /// Represents a user entity that receives a generic identifier.
    /// </summary>
    /// <typeparam name="TId">The identifier type.</typeparam>
    public class UserEntity<TId> : Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="UserEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        public UserEntity(TId id) : base(id)
        {
        }
    }

    /// <summary>
    /// Represents a product entity that receives a generic identifier.
    /// </summary>
    /// <typeparam name="TId">The identifier type.</typeparam>
    public class ProductEntity<TId> : Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Initiates a new instance of the <see cref="ProductEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        public ProductEntity(TId id) : base(id)
        {
        }
    }

    /// <summary>
    /// Represents a numeric entity that receives a generic identifier.
    /// </summary>
    /// <typeparam name="TId">The identifier type.</typeparam>
    public class NumericEntity<TId> : Entity<TId> where TId : notnull
    {
        /// <summary>
        /// Gets the numeric entity value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Initiates a new instance of the <see cref="NumericEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <param name="value">The numeric value.</param>
        public NumericEntity(TId id, int value) : base(id)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Represents a factory to create <see cref="UserEntity"/> instances.
    /// </summary>
    public static class UserEntity
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        /// <param name="dummyDomainEventsCount">
        /// The quantity of dummy domain events to be added to the user.
        /// </param>
        /// <returns>
        /// A new instance of the <see cref="UserEntity{TId}"/> class.
        /// </returns>
        public static UserEntity<T> Create<T>(
            T id,
            int dummyDomainEventsCount = 0
        ) where T : notnull
        {
            var user = new UserEntity<T>(id);

            for (var current = 0; current < dummyDomainEventsCount; current += 1)
            {
                user.AddDomainEvent(DummyDomainEvent.Create());
            }

            return user;
        }
    }

    /// <summary>
    /// Represents a factory to create <see cref="ProductEntity"/> instances.
    /// </summary>
    public static class ProductEntity
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ProductEntity{TId}"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the entity ID.</typeparam>
        /// <param name="id">The entity unique identifier.</param>
        /// <returns>
        /// A new instance of the <see cref="ProductEntity{TId}"/> class.
        /// </returns>
        public static ProductEntity<T> Create<T>(T id) where T : notnull
        {
            return new ProductEntity<T>(id);
        }
    }

    /// <summary>
    /// Represents a factory to create <see cref="NumericEntity"/> instances.
    /// </summary>
    public static class NumericEntity
    {
        /// <summary>
        /// Creates a new instance of the <see cref="NumericEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">the entity identifier.</param>
        /// <param name="value">The entity value.</param>
        /// <returns>
        /// A new instance of the <see cref="NumericEntity{TId}"/> class.
        /// </returns>
        public static NumericEntity<int> Create(int id = 1, int value = 1)
        {
            return new NumericEntity<int>(id, value);
        }
    }

    /// <summary>
    /// Represents a dummy domain event.
    /// </summary>
    public class DummyDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DummyDomainEvent"/> class.
        /// </summary>
        /// <returns>
        /// A new instance of the <see cref="DummyDomainEvent"/> class.
        /// </returns>
        public static DummyDomainEvent Create()
        {
            return new DummyDomainEvent();
        }
    }
}
