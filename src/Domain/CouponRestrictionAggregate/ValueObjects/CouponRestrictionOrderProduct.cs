using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate.ValueObjects;

/// <summary>
/// Represents a coupon restriction order product.
/// </summary>
public class CouponRestrictionOrderProduct : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;
    /// <summary>
    /// Gets the product category ids.
    /// </summary>
    public IEnumerable<CategoryId> CategoryIds { get; } = null!;

    private CouponRestrictionOrderProduct() { }

    private CouponRestrictionOrderProduct(ProductId productId, IEnumerable<CategoryId> categoryIds)
    {
        ProductId = productId;
        CategoryIds = categoryIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponRestrictionOrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="categoryIds">The category ids.</param>
    /// <returns>A new instance of the <see cref="CouponRestrictionOrderProduct"/> class.</returns>
    public static CouponRestrictionOrderProduct Create(ProductId productId, IEnumerable<CategoryId> categoryIds)
    {
        return new CouponRestrictionOrderProduct(productId, categoryIds);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return CategoryIds;
    }
}
