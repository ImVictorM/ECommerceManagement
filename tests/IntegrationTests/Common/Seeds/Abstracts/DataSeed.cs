
using Infrastructure.Common.Persistence;

using SharedKernel.Models;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides a base implementation for seed classes.
/// </summary>
/// <typeparam name="TEnum">The seed available data type.</typeparam>
/// <typeparam name="TEntity">The seed entity type.</typeparam>
/// <typeparam name="TEntityId">The seed entity identifier type.</typeparam>
public abstract class DataSeed<TEnum, TEntity, TEntityId>
    : IDataSeed<TEnum, TEntity, TEntityId>
    where TEnum : Enum
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    private readonly Dictionary<TEnum, TEntity> _data;

    /// <summary>
    /// Get the seed data.
    /// </summary>
    protected Dictionary<TEnum, TEntity> Data => _data;

    /// <inheritdoc/>
    public abstract int Order { get; }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DataSeed{TEnum, TEntity, TEntityId}"/> class.
    /// </summary>
    /// <param name="data">A dictionary containing the seed data.</param>
    protected DataSeed(Dictionary<TEnum, TEntity> data)
    {
        _data = data;
    }

    /// <inheritdoc/>
    public virtual TEntity GetEntity(TEnum key)
    {
        if (Data.TryGetValue(key, out var entity))
        {
            return entity;
        }

        throw new InvalidOperationException(
            $"{typeof(TEntity).Name} of type {key} has not been initialized."
        );
    }

    /// <inheritdoc/>
    public TEntityId GetEntityId(TEnum key)
    {
        return GetEntity(key).Id;
    }

    /// <inheritdoc/>
    public virtual IReadOnlyList<TEntity> ListAll(
        Func<TEntity, bool>? filter = null
    )
    {
        return filter != null
            ? Data.Values.Where(filter).ToList()
            : Data.Values.ToList();
    }

    /// <inheritdoc/>
    public abstract Task SeedAsync(IECommerceDbContext context);
}
