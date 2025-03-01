using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds;

/// <summary>
/// Defines an implementation for managing seed data in the integration tests.
/// </summary>
public sealed class SeedManager : ISeedManager
{
    private readonly IEnumerable<ISeed> _seeds;

    /// <summary>
    /// Initiates a new instance of the <see cref="SeedManager"/> class.
    /// </summary>
    /// <param name="seeds">The registered seeds.</param>
    public SeedManager(IEnumerable<ISeed> seeds)
    {
        _seeds = seeds;
    }

    /// <inheritdoc/>
    public TSeed GetSeed<TSeed>()
        where TSeed : ISeed
    {
        return _seeds.OfType<TSeed>().FirstOrDefault()
            ?? throw new InvalidOperationException(
                $"Seed of type {typeof(TSeed).Name} not found."
            );
    }

    /// <inheritdoc/>
    public async Task SeedAsync(IECommerceDbContext dbContext)
    {
        var orderedSeeds = _seeds.OrderBy(s => s.Order).ToList();

        foreach (var seed in orderedSeeds)
        {
            await seed.SeedAsync(dbContext);
        }

        await dbContext.SaveChangesAsync();
    }
}
