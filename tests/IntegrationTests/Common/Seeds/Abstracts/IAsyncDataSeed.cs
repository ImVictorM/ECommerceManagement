namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines an asynchronous seed initializer.
/// </summary>
public interface IAsyncDataSeed<TEnum, TEntity> : IDataSeed<TEnum, TEntity>
    where TEntity : class
    where TEnum : Enum
{
    /// <summary>
    /// Performs asynchronous initialization of the seed data.
    /// </summary>
    Task InitializeAsync();
}
