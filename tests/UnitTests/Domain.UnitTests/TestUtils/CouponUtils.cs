using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Coupon"/> class.
/// </summary>
public static class CouponUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Coupon"/> class.
    /// </summary>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">The coupon minimum price.</param>
    /// <param name="autoApply">The coupon auto apply flag.</param>
    /// <param name="active">The initial state of the coupon.</param>
    /// <returns>A new instance of the <see cref="Coupon"/> class.</returns>
    public static Coupon CreateCoupon(
        Discount? discount = null,
        string? code = null,
        int? usageLimit = null,
        decimal? minPrice = null,
        bool? autoApply = null,
        bool active = true
    )
    {
        var coupon = Coupon.Create(
            discount ?? DiscountUtils.CreateDiscount(PercentageUtils.Create(DomainConstants.Coupon.DiscountPercentage)),
            code ?? DomainConstants.Coupon.Code,
            usageLimit ?? DomainConstants.Coupon.UsageLimit,
            minPrice ?? DomainConstants.Coupon.MinPrice,
            autoApply ?? DomainConstants.Coupon.AutoApply
        );

        if (!active)
        {
            coupon.Deactivate();
        }

        return coupon;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrder"/> class.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="CouponOrder"/> class.</returns>
    public static CouponOrder CreateCouponOrder(
        HashSet<(ProductId ProductId, IReadOnlySet<CategoryId> Categories)>? products = null,
        decimal? total = null
    )
    {
        return CouponOrder.Create(
            products ??
                [
                    (ProductId.Create(1), new HashSet<CategoryId> { CategoryId.Create(2) })
                ],
            total ?? 1000m
        );
    }
}
