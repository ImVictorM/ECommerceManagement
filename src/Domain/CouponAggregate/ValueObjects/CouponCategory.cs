using Domain.CategoryAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon category.
/// </summary>
public class CouponCategory : ValueObject
{
    /// <summary>
    /// Gets the category identifier.
    /// </summary>
    public CategoryId CategoryId { get; } = null!;

    private CouponCategory() { }

    private CouponCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponCategory"/> class.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="CouponCategory"/> class.
    /// </returns>
    public static CouponCategory Create(CategoryId categoryId)
    {
        return new CouponCategory(categoryId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CategoryId;
    }
}
