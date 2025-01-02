using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils.Extensions;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Category"/> class.
/// </summary>
public static class CategoryUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="id">The category id.</param>
    /// <param name="name">The category name.</param>
    /// <returns>A new instance of the <see cref="Category"/> class.</returns>
    public static Category CreateCategory(
        CategoryId? id = null,
        string? name = null
    )
    {
        var category = Category.Create(
            name ?? _faker.Commerce.Categories(1).First()
        );

        if (id != null)
        {
            category.SetIdUsingReflection(id);
        }

        return category;
    }

    /// <summary>
    /// Creates a list containing random unique categories.
    /// </summary>
    /// <param name="count">The quantity of categories to be generated.</param>
    /// <returns>A list containing random unique categories.</returns>
    public static IEnumerable<Category> CreateCategories(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateCategory(id: CategoryId.Create(index + 1)));
    }
}
