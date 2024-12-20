using Domain.CategoryAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate.ValueObjects;

/// <summary>
/// Encapsulates the category id for a restricted category.
/// </summary>
public class CategoryRestricted : ValueObject
{
    /// <summary>
    /// Gets the category id.
    /// </summary>
    public CategoryId CategoryId { get; } = null!;

    private CategoryRestricted() { }

    private CategoryRestricted(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CategoryRestricted"/> class.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <returns>A new instance of the <see cref="CategoryRestricted"/> class.</returns>
    public static CategoryRestricted Create(CategoryId categoryId)
    {
        return new CategoryRestricted(categoryId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
