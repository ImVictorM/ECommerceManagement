using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate.ValueObjects;

/// <summary>
/// Represents a coupon restriction order.
/// </summary>
public class CouponRestrictionOrder : ValueObject
{
    /// <summary>
    /// Gets the order products.
    /// </summary>
    public IEnumerable<CouponRestrictionOrderProduct> Products { get; } = null!;

    private CouponRestrictionOrder() { }

    private CouponRestrictionOrder(IEnumerable<CouponRestrictionOrderProduct> products)
    {
        Products = products;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponRestrictionOrder"/> class.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <returns>A new instance of the <see cref="CouponRestrictionOrder"/> class.</returns>
    public static CouponRestrictionOrder Create(IEnumerable<CouponRestrictionOrderProduct> products)
    {
        return new CouponRestrictionOrder(products);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Products;
    }
}

