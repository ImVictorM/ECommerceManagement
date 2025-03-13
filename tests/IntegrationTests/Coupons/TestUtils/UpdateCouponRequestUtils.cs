using Contracts.Common;
using Contracts.Coupons;
using Contracts.Coupons.Restrictions;

using IntegrationTests.TestUtils.Contracts;

using Bogus;

namespace IntegrationTests.Coupons.TestUtils;

/// <summary>
/// Utilities for the <see cref="UpdateCouponRequest"/> class.
/// </summary>
public static class UpdateCouponRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="UpdateCouponRequest"/> class.
    /// </summary>
    /// <param name="discount">The new coupon discount.</param>
    /// <param name="code">The new coupon code.</param>
    /// <param name="usageLimit">The new coupon usage limit.</param>
    /// <param name="autoApply">
    /// A boolean flag indicating if the coupon should auto apply.
    /// </param>
    /// <param name="minPrice">The new coupon minimum price.</param>
    /// <param name="restrictions">The new coupon restrictions (optional).</param>
    /// <returns>
    /// A new instance of the <see cref="UpdateCouponRequest"/> class.
    /// </returns>
    public static UpdateCouponRequest CreateRequest(
        DiscountContract? discount = null,
        string? code = null,
        int? usageLimit = null,
        bool autoApply = false,
        decimal? minPrice = 0m,
        IEnumerable<CouponRestriction>? restrictions = null
    )
    {
        return new UpdateCouponRequest(
            discount ?? DiscountContractUtils.CreateDiscountValidToDate(),
            code ?? _faker.Lorem.Word(),
            usageLimit ?? _faker.Random.Int(1, 5000),
            autoApply,
            minPrice ?? _faker.Random.Decimal(0m, 500m),
            restrictions
        );
    }
}
