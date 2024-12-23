using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon order product.
/// </summary>
public class CouponOrderProduct : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;
    /// <summary>
    /// Gets the product category ids.
    /// </summary>
    public IEnumerable<CategoryId> CategoryIds { get; } = null!;

    private CouponOrderProduct() { }

    private CouponOrderProduct(ProductId productId, IEnumerable<CategoryId> categoryIds)
    {
        ProductId = productId;
        CategoryIds = categoryIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="categoryIds">The product category ids.</param>
    /// <returns>A new instance of the <see cref="CouponOrderProduct"/> class.</returns>
    public static CouponOrderProduct Create(ProductId productId, IEnumerable<CategoryId> categoryIds)
    {
        return new CouponOrderProduct(productId, categoryIds);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return CategoryIds;
    }
}
