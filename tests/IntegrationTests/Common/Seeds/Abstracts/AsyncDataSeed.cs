using SharedKernel.Models;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides a base implementation for asynchronous seed classes.
/// </summary>
public abstract class AsyncDataSeed<TEnum, TEntity, TEntityId>
    : DataSeed<TEnum, TEntity, TEntityId>, IAsyncDataSeed<TEnum, TEntity, TEntityId>
    where TEnum : Enum
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="AsyncDataSeed{TEnum, TEntity, TEntityId}"/> with an empty
    /// seed data.
    /// </summary>
    protected AsyncDataSeed() : base([])
    {
    }

    /// <inheritdoc/>
    public abstract Task InitializeAsync();
}
