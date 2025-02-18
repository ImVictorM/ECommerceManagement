using Infrastructure.Common.Persistence;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines a contract for managing seed data in the integration tests.
/// </summary>
public interface ISeedManager
{
    /// <summary>
    /// Retrieves a seed instance by its <see cref="IDataSeed{TEnum, TEntity}"/> interface.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum used to identify seed data.</typeparam>
    /// <typeparam name="TEntity">The type of the entity associated with the seed data.</typeparam>
    /// <returns>An instance of <see cref="IDataSeed{TEnum, TEntity}"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no seed of the specified type is found.
    /// </exception>
    IDataSeed<TEnum, TEntity> GetSeed<TEnum, TEntity>()
        where TEnum : Enum
        where TEntity : class;

    /// <summary>
    /// Retrieves a seed instance by its concrete type.
    /// </summary>
    /// <typeparam name="T">The seed concrete type.</typeparam>
    /// <returns>An instance of <see cref="ISeed"/>.</returns>
    T GetSeed<T>() where T : ISeed;

    /// <summary>
    /// Seeds the database with all registered seed data.
    /// </summary>
    /// <param name="dbContext">The database context to seed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SeedAsync(ECommerceDbContext dbContext);
}
