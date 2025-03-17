using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Categories;
using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Coupons;

/// <summary>
/// Provides seed data for coupons in the database.
/// </summary>
public sealed class CouponSeed
    : DataSeed<CouponSeedType, Coupon, CouponId>, ICouponSeed
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CouponSeed"/> class.
    /// </summary>
    /// <param name="categorySeed">The category seed.</param>
    public CouponSeed(ICategorySeed categorySeed)
        : base(CreateSeedData(categorySeed))
    {
        Order = categorySeed.Order + 20;
    }

    private static Dictionary<CouponSeedType, Coupon> CreateSeedData(
        ICategorySeed categorySeed
    )
    {
        return new()
        {
            [CouponSeedType.TECH_COUPON] = CouponUtils.CreateCoupon
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
                    CouponCategoryRestriction.Create(
                        categoriesAllowed:
                        [
                            CouponCategory.Create(categorySeed.GetEntityId(
                                CategorySeedType.TECHNOLOGY
                            ))
                        ],
                        productsFromCategoryNotAllowed: []
                    )
                ]
            ),
            [CouponSeedType.PAST10] = CouponUtils.CreateCoupon(
                id: CouponId.Create(-2),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(10),
                    description: "10 percent discount",
                    startingDate: DateTimeOffset.UtcNow.AddHours(-10),
                    endingDate: DateTimeOffset.UtcNow.AddHours(-4)
                ),
                code: "DISCOUNT10",
                usageLimit: 100,
                autoApply: false,
                minPrice: 0m,
                initialRestrictions: []
            ),
            [CouponSeedType.SUMMER15] = CouponUtils.CreateCoupon(
                id: CouponId.Create(-3),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(15),
                    description: "15 percent discount",
                    startingDate: DateTimeOffset.UtcNow.AddDays(1),
                    endingDate: DateTimeOffset.UtcNow.AddMonths(2)
                ),
                code: "SUMMER15",
                usageLimit: 200,
                autoApply: false,
                minPrice: 75.00m,
                initialRestrictions: []
            ),
            [CouponSeedType.WELCOME20_INACTIVE] = CouponUtils.CreateCoupon(
                id: CouponId.Create(-4),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(20),
                    description: "20 percent discount",
                    startingDate: DateTimeOffset.UtcNow.AddDays(1),
                    endingDate: DateTimeOffset.UtcNow.AddDays(2)
                ),
                code: "WELCOME20",
                usageLimit: 200,
                autoApply: false,
                minPrice: 75.00m,
                initialRestrictions: [],
                active: false
            ),
            [CouponSeedType.BLACKFRIDAY50] = CouponUtils.CreateCoupon
            (
                id: CouponId.Create(-5),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(5),
                    description: "tech discount",
                    startingDate: DateTimeOffset.UtcNow.AddHours(-10),
                    endingDate: DateTimeOffset.UtcNow.AddDays(5)
                ),
                code: "BLACK50",
                usageLimit: 500,
                minPrice: 1500m,
                initialRestrictions: [
                    CouponCategoryRestriction.Create(
                        categoriesAllowed:
                        [
                            CouponCategory.Create(categorySeed.GetEntityId(
                                CategorySeedType.FASHION
                            ))
                        ],
                        productsFromCategoryNotAllowed: []
                    )
                ]
            )
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Coupons.AddRangeAsync(ListAll());
    }
}
