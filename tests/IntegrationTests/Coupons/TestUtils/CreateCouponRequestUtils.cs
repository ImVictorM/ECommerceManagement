using Contracts.Common;
using Contracts.Coupons;
using Contracts.Coupons.Restrictions;

using IntegrationTests.TestUtils.Contracts;

using Bogus;

namespace IntegrationTests.Coupons.TestUtils;

/// <summary>
/// Utilities for the <see cref="CreateCouponRequest"/> class.
/// </summary>
public static class CreateCouponRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="CreateCouponRequest"/> class.
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
    /// A new instance of the <see cref="CreateCouponRequest"/> class.
    /// </returns>
    public static CreateCouponRequest CreateRequest(
        DiscountContract? discount = null,
        string? code = null,
        int? usageLimit = null,
        decimal? minPrice = null,
        IEnumerable<CouponRestriction>? restrictions = null,
        bool autoApply = false
    )
    {
        return new CreateCouponRequest(
            discount ?? DiscountContractUtils.CreateDiscount(),
            code ?? "DefaultCode",
            usageLimit ?? _faker.Random.Int(20, 200),
            autoApply,
            minPrice ?? 0m,
            restrictions
        );
    }
}
