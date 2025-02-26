
using Infrastructure.Common.Persistence;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides a base implementation for seed classes.
/// </summary>
/// <typeparam name="TEnum">The seed available data type.</typeparam>
/// <typeparam name="TEntity">The seed entity type.</typeparam>
public abstract class DataSeed<TEnum, TEntity> : IDataSeed<TEnum, TEntity>
    where TEnum : Enum
    where TEntity : class
{
    private readonly Dictionary<TEnum, TEntity> _data;

    /// <summary>
    /// Get the seed data.
    /// </summary>
    protected Dictionary<TEnum, TEntity> Data => _data;

    /// <inheritdoc/>
    public abstract int Order { get; }

    /// <summary>
    /// Initializes a new instance with an existing dictionary.
    /// </summary>
    /// <param name="data">A dictionary containing the seed data.</param>
    protected DataSeed(Dictionary<TEnum, TEntity> data)
    {
        _data = data;
    }

    /// <summary>
    /// Retrieves a seed entity by key.
    /// </summary>
    /// <param name="key">The entity type.</param>
    /// <returns>The entity data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the data was not initialized correctly.</exception>
    public virtual TEntity GetByType(TEnum key)
    {
        if (Data.TryGetValue(key, out var entity))
        {
            return entity;
        }

        throw new InvalidOperationException($"{typeof(TEntity).Name} of type {key} has not been initialized.");
    }

    /// <summary>
    /// Lists all seed entities.
    /// </summary>
    /// <returns>A list containing all entities.</returns>
    public virtual IReadOnlyList<TEntity> ListAll(Func<TEntity, bool>? filter = null)
    {
        return filter != null
            ? Data.Values.Where(filter).ToList()
            : Data.Values.ToList();
    }

    /// <inheritdoc/>
    public abstract Task SeedAsync(IECommerceDbContext context);
}
