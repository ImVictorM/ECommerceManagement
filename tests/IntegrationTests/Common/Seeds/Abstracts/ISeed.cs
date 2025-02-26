using Infrastructure.Common.Persistence;

namespace IntegrationTests.Common.Seeds.Abstracts;

/// <summary>
/// Defines seed persistence operations based on dependency order.
/// </summary>
public interface ISeed
{
    /// <summary>
    /// Seeds the database into the provided <paramref name="context"/>.
    /// </summary>
    /// <param name="context">The database context.</param>
    Task SeedAsync(IECommerceDbContext context);

    /// <summary>
    /// The order in which the seed should be applied.
    /// Lower numbers represents a seed with minimal dependencies on relationships.
    /// Higher numbers represents a seed with dependencies on relationships.
    /// </summary>
    int Order { get; }
}
