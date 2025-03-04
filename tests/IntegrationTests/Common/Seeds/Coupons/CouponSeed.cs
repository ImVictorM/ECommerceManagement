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
                    CategoryRestriction.Create(
                        categoriesAllowed:
                        [
                            CouponCategory.Create(categorySeed.GetEntityId(
                                CategorySeedType.TECHNOLOGY
                            ))
                        ],
                        productsFromCategoryNotAllowed: []
                    )
                ],
                active: true
            )
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Coupons.AddRangeAsync(ListAll());
    }
}
