using Application.Coupons.Commands.CreateCoupon;
using Application.Coupons.DTOs.Restrictions;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using Bogus;

namespace Application.UnitTests.Coupons.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateCouponCommand"/> class.
/// </summary>
public static class CreateCouponCommandUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateCouponCommand"/> class.
    /// </summary>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">
    /// The coupon minimum price. The default value is 0.
    /// </param>
    /// <param name="restrictions">The coupon restrictions.</param>
    /// <param name="autoApply">
    /// A boolean indicating if the coupon should auto apply.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="CreateCouponCommand"/> class.
    /// </returns>
    public static CreateCouponCommand CreateCommand(
        Discount? discount = null,
        string? code = null,
        int? usageLimit = null,
        decimal? minPrice = null,
        IEnumerable<CouponRestrictionIO>? restrictions = null,
        bool autoApply = false
    )
    {
        return new CreateCouponCommand(
            discount ?? DiscountUtils.CreateDiscount(),
            code ?? "DefaultCode",
            usageLimit ?? _faker.Random.Int(20, 200),
            autoApply,
            minPrice ?? 0m,
            restrictions
        );
    }
}
