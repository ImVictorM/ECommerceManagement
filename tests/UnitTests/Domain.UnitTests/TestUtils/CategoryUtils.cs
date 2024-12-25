using Domain.CategoryAggregate;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Category"/> class.
/// </summary>
public static class CategoryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="name">The category name.</param>
    /// <returns>A new instance of the <see cref="Category"/> class.</returns>
    public static Category CreateCategory(string? name = null)
    {
        return Category.Create(name ?? DomainConstants.Category.Name);
    }
}
