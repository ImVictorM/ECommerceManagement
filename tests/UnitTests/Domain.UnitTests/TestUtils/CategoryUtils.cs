using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.UnitTests.TestUtils.Extensions;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Category"/> class.
/// </summary>
public static class CategoryUtils
{
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
        var category = Category.Create(name ?? DomainConstants.Category.Name);

        if (id != null)
        {
            category.SetIdUsingReflection(id);
        }

        return category;
    }
}
