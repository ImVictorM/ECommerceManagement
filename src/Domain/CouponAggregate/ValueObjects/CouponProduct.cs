using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.CouponAggregate.ValueObjects;

/// <summary>
/// Represents a coupon product.
/// </summary>
public class CouponProduct : ValueObject
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private CouponProduct() { }

    private CouponProduct(ProductId productId)
    {
        ProductId = productId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponProduct"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="CouponProduct"/> class.
    /// </returns>
    public static CouponProduct Create(ProductId productId)
    {
        return new CouponProduct(productId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
    }
}
