using Domain.CategoryAggregate;
using Domain.UnitTests.TestUtils;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// The available categories in the database seed.
/// </summary>
public enum SeedAvailableCategories
{
    /// <summary>
    /// Represents a technology category.
    /// </summary>
    TECHNOLOGY,
    /// <summary>
    /// Represents a sports category.
    /// </summary>
    SPORTS,
    /// <summary>
    /// Represents a books and stationery category.
    /// </summary>
    BOOKS_STATIONERY,
    /// <summary>
    /// Represents a fashion category.
    /// </summary>
    FASHION,
    /// <summary>
    /// Represents an office supplies category.
    /// </summary>
    OFFICE_SUPPLIES,
    /// <summary>
    /// Represents an jewelry category.
    /// </summary>
    JEWELRY
}

/// <summary>
/// Contain category seed data.
/// </summary>
public static class CategorySeed
{
    private static readonly Dictionary<SeedAvailableCategories, Category> _categories = new Dictionary<SeedAvailableCategories, Category>()
    {
        [SeedAvailableCategories.TECHNOLOGY] = CategoryUtils.CreateCategory("technology"),
        [SeedAvailableCategories.SPORTS] = CategoryUtils.CreateCategory("sports"),
        [SeedAvailableCategories.BOOKS_STATIONERY] = CategoryUtils.CreateCategory("books_stationery"),
        [SeedAvailableCategories.FASHION] = CategoryUtils.CreateCategory("fashion"),
        [SeedAvailableCategories.OFFICE_SUPPLIES] = CategoryUtils.CreateCategory("office_supplies"),
        [SeedAvailableCategories.JEWELRY] = CategoryUtils.CreateCategory("jewelry"),
    };

    /// <summary>
    /// List all the seed categories.
    /// </summary>
    /// <param name="filter">A filter to list specific categories.</param>
    public static IEnumerable<Category> ListCategories(Func<Category, bool>? filter = null)
    {
        return filter != null ? _categories.Values.Where(filter) : _categories.Values;
    }

    /// <summary>
    /// Retrieves a seed category by type.
    /// </summary>
    /// <param name="categoryType">The category type.</param>
    /// <returns>The category.</returns>
    public static Category GetSeedCategory(SeedAvailableCategories categoryType)
    {
        return _categories[categoryType];
    }
}
