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
    public IDataSeed<TEnum, TEntity> GetSeed<TEnum, TEntity>()
        where TEnum : Enum
        where TEntity : class
    {
        return _seeds.OfType<IDataSeed<TEnum, TEntity>>().FirstOrDefault()
            ?? throw new InvalidOperationException($"Seed for {typeof(TEntity).Name} not found.");
    }

    /// <inheritdoc/>
    public T GetSeed<T>() where T : ISeed
    {
        return _seeds.OfType<T>().FirstOrDefault()
            ?? throw new InvalidOperationException($"Seed of type {typeof(T).Name} not found.");
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
