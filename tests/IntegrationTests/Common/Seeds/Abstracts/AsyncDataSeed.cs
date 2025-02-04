namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Provides a base implementation for asynchronous seed classes.
/// </summary>
public abstract class AsyncDataSeed<TEnum, TEntity> : DataSeed<TEnum, TEntity>, IAsyncDataSeed<TEnum, TEntity>
    where TEntity : class
    where TEnum : Enum
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncDataSeed{TEnum, TEntity}"/> with an empty seed data.
    /// </summary>
    protected AsyncDataSeed() : base([])
    {
    }

    /// <inheritdoc/>
    public abstract Task InitializeAsync();
}
