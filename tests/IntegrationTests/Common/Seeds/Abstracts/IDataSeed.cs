using SharedKernel.Models;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines the basic operations for seed data.
/// </summary>
/// <typeparam name="TEnum">
/// The type of the key used to index seed data.
/// </typeparam>
/// <typeparam name="TEntity">
/// The type of the seed entity.
/// </typeparam>
/// <typeparam name="TEntityId">
/// The type of the entity id.
/// </typeparam>
public interface IDataSeed<TEnum, TEntity, TEntityId> : ISeed
    where TEnum : Enum
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Retrieves a seed entity by key.
    /// </summary>
    /// <param name="key">The key of the <typeparamref name="TEnum"/>.</param>
    /// <returns>The seed entity.</returns>
    TEntity GetEntity(TEnum key);

    /// <summary>
    /// Retrieves a seed entity id by key.
    /// </summary>
    /// <param name="key">The key of the <typeparamref name="TEnum"/>.</param>
    /// <returns>The seed entity id.</returns>
    TEntityId GetEntityId(TEnum key);

    /// <summary>
    /// Lists all seed entities.
    /// </summary>
    /// <param name="filter">A filter to be applied to the entities.</param>
    IReadOnlyList<TEntity> ListAll(Func<TEntity, bool>? filter = null);
}
