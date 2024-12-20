using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate.ValueObjects;

/// <summary>
/// Encapsulates the product id for a restricted product.
/// </summary>
public class ProductRestricted : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private ProductRestricted() { }

    private ProductRestricted(ProductId productId)
    {
        ProductId = productId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductRestricted"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>A new instance of the <see cref="ProductRestricted"/> class.</returns>
    public static ProductRestricted Create(ProductId productId)
    {
        return new ProductRestricted(productId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
    }
}
