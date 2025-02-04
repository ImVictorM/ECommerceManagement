using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Categories;

/// <summary>
/// Provides seed data for carriers in the database.
/// </summary>
public sealed class CategorySeed : DataSeed<CategorySeedType, Category>
{
    private static Dictionary<CategorySeedType, Category> _categories => new()
    {
        [CategorySeedType.TECHNOLOGY] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-1),
            name: "technology"
        ),
        [CategorySeedType.SPORTS] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-2),
            name: "sports"
        ),
        [CategorySeedType.BOOKS_STATIONERY] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-3),
            name: "books_stationery"
        ),
        [CategorySeedType.FASHION] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-4),
            name: "fashion"
        ),
        [CategorySeedType.OFFICE_SUPPLIES] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-5),
            name: "office_supplies"
        ),
        [CategorySeedType.JEWELRY] = CategoryUtils.CreateCategory
        (
            id: CategoryId.Create(-6),
            name: "jewelry"
        ),
    };

    /// <inheritdoc/>
    public override int Order => 10;

    /// <summary>
    /// Initiates a new instance of the <see cref="CategorySeed"/> class.
    /// </summary>
    public CategorySeed() : base(_categories)
    {
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.Categories.AddRangeAsync(ListAll());
    }
}
