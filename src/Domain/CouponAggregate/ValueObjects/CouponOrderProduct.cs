using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon order product.
/// </summary>
public sealed class CouponOrderProduct : ValueObject
{
    private readonly IReadOnlySet<CategoryId> _productCategoryIds = null!;

    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    /// <summary>
    /// Gets the product category identifiers.
    /// </summary>
    public IReadOnlySet<CategoryId> ProductCategoryIds => _productCategoryIds;

    private CouponOrderProduct() { }

    private CouponOrderProduct(
        ProductId productId,
        IReadOnlySet<CategoryId> productCategoryIds
    )
    {
        ProductId = productId;
        _productCategoryIds = productCategoryIds;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="productCategoryIds">The product category identifiers.</param>
    /// <returns>
    /// A new instance of the <see cref="CouponOrderProduct"/> class.
    /// </returns>
    public static CouponOrderProduct Create(
        ProductId productId,
        IReadOnlySet<CategoryId> productCategoryIds
    )
    {
        return new CouponOrderProduct(productId, productCategoryIds);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;

        foreach (var categoryId in _productCategoryIds)
        {
            yield return categoryId;
        }
    }
}
