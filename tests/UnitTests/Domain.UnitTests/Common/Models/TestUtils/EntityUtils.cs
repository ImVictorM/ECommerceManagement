using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace Domain.UnitTests.Common.Models.TestUtils;

/// <summary>
/// Entity utilities.
/// </summary>
public static class EntityUtils
{
    /// <summary>
    /// Represents a user entity that receives a generic id.
    /// </summary>
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
    /// Represents a product entity that receives a generic id.
    /// </summary>
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
    /// Create instances of the generic UserEntity.
    /// </summary>
    public static class UserEntity
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserEntity{TId}"/> class.
        /// </summary>
        /// <param name="id">The entity unique identifier.</param>
        /// <param name="dummyDomainEventsCount">Quantity of dummy domain events to be added. Default is 0.</param>
        /// <returns>A new instance of the <see cref="UserEntity{TId}"/> class.</returns>
        public static UserEntity<T> Create<T>(T id, int dummyDomainEventsCount = 0) where T : notnull
        {
            var user = new UserEntity<T>(id);

            for (var current = 0; current < dummyDomainEventsCount; current++)
            {
                user.AddDomainEvent(DummyDomainEvent.Create());
            }

            return user;
        }
    }

    /// <summary>
    /// Create instances of the generic ProductEntity.
    /// </summary>
    public static class ProductEntity
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ProductEntity{TId}"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the entity ID.</typeparam>
        /// <param name="id">The entity unique identifier.</param>
        /// <returns>A new instance of the <see cref="ProductEntity{TId}"/> class.</returns>
        public static ProductEntity<T> Create<T>(T id) where T : notnull
        {
            return new ProductEntity<T>(id);
        }
    }

    /// <summary>
    /// Represents a dummy domain event.
    /// </summary>
    public class DummyDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Creates a dummy domain event with the type <see cref="DummyDomainEvent"/>.
        /// </summary>
        /// <returns>A dummy domain event with the type <see cref="DummyDomainEvent"/>.</returns>
        public static DummyDomainEvent Create()
        {
            return new DummyDomainEvent();
        }
    }
}
