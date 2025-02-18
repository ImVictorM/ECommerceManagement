namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines the basic operations for seed data.
/// </summary>
/// <typeparam name="TEnum">The type of the key used to index seed data.</typeparam>
/// <typeparam name="TEntity">The type of the seed entity.</typeparam>
public interface IDataSeed<TEnum, TEntity> : ISeed
    where TEnum : Enum
    where TEntity : class
{
    /// <summary>
    /// Retrieves a seed entity by key.
    /// </summary>
    TEntity GetByType(TEnum key);
    /// <summary>
    /// Lists all seed entities.
    /// </summary>
    /// <param name="filter">A filter to be applied to the entities.</param>
    IReadOnlyList<TEntity> ListAll(Func<TEntity, bool>? filter = null);
}
