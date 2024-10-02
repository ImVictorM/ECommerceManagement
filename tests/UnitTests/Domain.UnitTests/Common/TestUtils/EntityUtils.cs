using Domain.Common.Interfaces;
using Domain.Common.Models;

namespace Domain.UnitTests.Common.TestUtils;

/// <summary>
/// Entity utilities.
/// </summary>
public static class EntityUtils
{
    private class UserEntity<TEntityId> : Entity<TEntityId> where TEntityId : notnull
    {
        public UserEntity(TEntityId id) : base(id)
        {

        }
    }

    private class ProductEntity<TEntityId> : Entity<TEntityId> where TEntityId : notnull
    {
        public ProductEntity(TEntityId id) : base(id)
        {

        }
    }

    private class DummyDomainEvent : IDomainEvent { }

    /// <summary>
    /// Creates a new user of type <see cref="UserEntity{TEntityId}"/> with the specified identifier.
    /// </summary>
    /// <typeparam name="TId">The identifier type.</typeparam>
    /// <param name="id">The identifier value.</param>
    /// <returns>A new user of type <see cref="UserEntity{TEntityId}"/> with the specified identifier.</returns>
    public static Entity<TId> CreateUserEntity<TId>(TId id) where TId : notnull
    {
        return new UserEntity<TId>(id);
    }

    /// <summary>
    /// Creates a new product of type <see cref="ProductEntity{TEntityId}"/> with the specified identifier.
    /// </summary>
    /// <typeparam name="TId">The identifier type.</typeparam>
    /// <param name="id">The identifier value.</param>
    /// <returns>A new product of type <see cref="ProductEntity{TEntityId}"/> with the specified identifier.</returns>
    public static Entity<TId> CreateProductEntity<TId>(TId id) where TId: notnull
    {
        return new ProductEntity<TId>(id);
    }

    /// <summary>
    /// Creates a dummy domain event with the type <see cref="DummyDomainEvent"/>.
    /// </summary>
    /// <returns>A dummy domain event with the type <see cref="DummyDomainEvent"/>.</returns>
    public static IDomainEvent CreateDummyDomainEvent()
    {
        return new DummyDomainEvent();
    }
}
