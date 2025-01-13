using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// The available coupons in the database seed.
/// </summary>
public enum SeedAvailableCoupons
{
    /// <summary>
    /// Represents a technology coupon.
    /// </summary>
    TECH_COUPON
}

/// <summary>
/// Contain coupon seed data.
/// </summary>
public static class CouponSeed
{
    private static readonly Dictionary<SeedAvailableCoupons, Coupon> _coupons = new()
    {
        [SeedAvailableCoupons.TECH_COUPON] = CouponUtils
        .CreateCoupon
            (
                id: CouponId.Create(-1),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(5),
                    description: "tech discount",
                    startingDate: DateTimeOffset.UtcNow.AddHours(-10),
                    endingDate: DateTimeOffset.UtcNow.AddDays(5)
                ),
                code: "TECH",
                usageLimit: 500,
                minPrice: 0m,
                initialRestrictions: [
                    CategoryRestriction.Create(
                        categoriesAllowed:
                        [
                            CouponCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.TECHNOLOGY).Id)
                        ],
                        productsFromCategoryNotAllowed: []
                    )
                ],
                active: true
            )
    };

    /// <summary>
    /// List all the seed coupons.
    /// </summary>
    public static IReadOnlyList<Coupon> ListCoupons()
    {
        return [.. _coupons.Values];
    }

    /// <summary>
    /// Retrieves a seed coupon by type.
    /// </summary>
    /// <param name="couponType">The coupon type.</param>
    /// <returns>The coupon.</returns>
    public static Coupon GetSeedCoupon(SeedAvailableCoupons couponType)
    {
        return _coupons[couponType];
    }
}
