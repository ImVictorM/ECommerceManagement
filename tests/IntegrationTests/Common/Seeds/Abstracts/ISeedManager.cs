using Infrastructure.Common.Persistence;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines a contract for managing seed data in the integration tests.
/// </summary>
public interface ISeedManager
{
    /// <summary>
    /// Retrieves a seed instance by its interface.
    /// </summary>
    /// <typeparam name="TSeed">
    /// The seed type implementing <see cref="ISeed"/>.
    /// </typeparam>
    /// <returns>An instance of <see cref="ISeed"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no seed of the specified type is found.
    /// </exception>
    TSeed GetSeed<TSeed>() where TSeed : ISeed;

    /// <summary>
    /// Seeds the database with all registered seed data.
    /// </summary>
    /// <param name="dbContext">The database context to seed.</param>
    Task SeedAsync(IECommerceDbContext dbContext);
}
