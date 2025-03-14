using Application.Coupons.Commands.UpdateCoupon;
using Application.Coupons.DTOs.Restrictions;

using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Bogus;

namespace Application.UnitTests.Coupons.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateCouponCommand"/> class.
/// </summary>
public static class UpdateCouponCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateCouponCommand"/> class.
    /// </summary>
    /// <param name="couponId">The coupon identifier.</param>
    /// <param name="discount">The new coupon discount.</param>
    /// <param name="code">The new coupon code.</param>
    /// <param name="usageLimit">The new coupon usage limit.</param>
    /// <param name="minPrice">The new coupon minimum price.</param>
    /// <param name="restrictions">The new coupon restrictions.</param>
    /// <param name="autoApply">The new coupon auto apply value.</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateCouponCommand"/> class.
    /// </returns>
    public static UpdateCouponCommand CreateCommand(
        string? couponId = null,
        Discount? discount = null,
        string? code = null,
        int? usageLimit = null,
        decimal? minPrice = null,
        IEnumerable<CouponRestrictionIO>? restrictions = null,
        bool autoApply = false
    )
    {
        return new UpdateCouponCommand(
            couponId ?? NumberUtils.CreateRandomLongAsString(),
            discount ?? DiscountUtils.CreateDiscount(),
            code ?? _faker.Random.Word(),
            usageLimit ?? _faker.Random.Int(1, 100),
            autoApply,
            minPrice ?? _faker.Random.Decimal(0, 100),
            restrictions
        );
    }
}
