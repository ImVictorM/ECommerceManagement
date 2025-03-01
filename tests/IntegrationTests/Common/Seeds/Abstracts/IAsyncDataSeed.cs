using SharedKernel.Models;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines an asynchronous seed initializer.
/// </summary>
public interface IAsyncDataSeed<TEnum, TEntity, TEntityId>
    : IDataSeed<TEnum, TEntity, TEntityId>
    where TEnum : Enum
    where TEntity : AggregateRoot<TEntityId>
    where TEntityId : notnull
{
    /// <summary>
    /// Performs asynchronous initialization of the seed data.
    /// </summary>
    Task InitializeAsync();
}
