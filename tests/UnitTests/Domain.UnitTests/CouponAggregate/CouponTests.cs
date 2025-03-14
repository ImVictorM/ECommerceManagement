using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.ValueObjects;
using SharedKernel.Extensions;
using SharedKernel.UnitTests.TestUtils;

using FluentAssertions;
using Bogus;

namespace Domain.UnitTests.CouponAggregate;

/// <summary>
/// Unit tests for the <see cref="Coupon"/> class.
/// </summary>
public class CouponTests
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Provides valid parameters to create new coupons.
    /// </summary>
    public static IEnumerable<object[]> GetCouponValidParameters()
    {
        foreach (var _ in Enumerable.Range(0, 5))
        {
            yield return new object[]
            {
                DiscountUtils.CreateDiscountValidToDate(),
                _faker.Random.Word(),
                _faker.Random.Int(1, 1000),
                _faker.Random.Decimal(0m, 500m),
                _faker.Random.Bool()
            };
        }
    }

    /// <summary>
    /// Verifies the coupon is created correctly.
    /// </summary>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">The coupon minimum price.</param>
    /// <param name="autoApply">
    /// A boolean indicating if the coupon should auto apply.
    /// </param>
    [Theory]
    [MemberData(nameof(GetCouponValidParameters))]
    public void CreateCoupon_WithValidParameters_CreatesWithoutThrowing(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice,
        bool autoApply
    )
    {
        var actionResult = FluentActions
            .Invoking(() => Coupon.Create(
                discount,
                code,
                usageLimit,
                minPrice,
                autoApply
            ))
            .Should()
            .NotThrow();

        var coupon = actionResult.Subject;

        coupon.Discount.Should().Be(discount);
        coupon.Code.Should().Be(code.ToUpperSnakeCase());
        coupon.UsageLimit.Should().Be(usageLimit);
        coupon.MinPrice.Should().Be(minPrice);
        coupon.AutoApply.Should().Be(autoApply);
        coupon.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// Verifies a coupon is updated correctly.
    /// </summary>
    /// <param name="discount">The new coupon discount.</param>
    /// <param name="code">The new coupon code.</param>
    /// <param name="usageLimit">The new coupon usage limit.</param>
    /// <param name="minPrice">The new coupon minimum price.</param>
    /// <param name="autoApply">The new auto apply flag.</param>
    [Theory]
    [MemberData(nameof(GetCouponValidParameters))]
    public void Update_WithValidParameters_UpdatesTheCouponInformation(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice,
        bool autoApply
    )
    {
        var couponToBeUpdated = CouponUtils.CreateCoupon();

        couponToBeUpdated.Update(
            discount,
            code,
            usageLimit,
            minPrice,
            autoApply
        );

        couponToBeUpdated.Discount.Should().Be(discount);
        couponToBeUpdated.Code.Should().Be(code.ToUpperSnakeCase());
        couponToBeUpdated.UsageLimit.Should().Be(usageLimit);
        couponToBeUpdated.MinPrice.Should().Be(minPrice);
        couponToBeUpdated.AutoApply.Should().Be(autoApply);
    }

    /// <summary>
    /// Provides a collection containing coupon restrictions.
    /// </summary>
    public static readonly IEnumerable<object[]> CouponRestrictions =
    [
        [
            CouponCategoryRestriction.Create(
                [
                    CouponCategory.Create(CategoryId.Create(1)),
                    CouponCategory.Create(CategoryId.Create(2))
                ]
            )
        ],
        [
            CouponProductRestriction.Create(
                [
                    CouponProduct.Create(ProductId.Create(1)),
                ]
            )
        ],
    ];

    /// <summary>
    /// Verifies it is possible to assign different kind of restrictions to a coupon.
    /// </summary>
    /// <param name="restriction">The restriction.</param>
    [Theory]
    [MemberData(nameof(CouponRestrictions))]
    public void AssignRestriction_WithDifferentRestrictions_AssignsCorrectly(
        CouponRestriction restriction
    )
    {
        var coupon = CouponUtils.CreateCoupon();

        coupon.AssignRestriction(restriction);

        coupon.Restrictions.Should().Contain(restriction);
    }

    /// <summary>
    /// Verifies the <see cref="Coupon.ClearRestrictions"/> method removes all
    /// restrictions from a coupon correctly.
    /// </summary>
    [Fact]
    public void ClearRestrictions_WithMultipleRestrictions_RemovesThemAll()
    {
        var coupon = CouponUtils.CreateCoupon();

        coupon.AssignRestriction(CouponCategoryRestriction.Create([
            CouponCategory.Create(CategoryId.Create(1)),
            CouponCategory.Create(CategoryId.Create(2))
        ]));

        coupon.AssignRestriction(CouponProductRestriction.Create([
            CouponProduct.Create(ProductId.Create(1)),
        ]));

        coupon.ClearRestrictions();

        coupon.Restrictions.Should().BeEmpty();
    }

    /// <summary>
    /// Provides a collection with coupon orders and expected result when calling the
    /// <see cref="Coupon.CanBeApplied(CouponOrder)"/> method.
    /// </summary>
    public static IEnumerable<object[]> CanOrderApplyCoupon =>
    [
        [
            CouponOrder.Create(
                [
                    CouponOrderProduct.Create(
                        ProductId.Create(1),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    )
                ],
                1m
            ),
            false
        ],
        [
            CouponOrder.Create(
                [
                    CouponOrderProduct.Create(
                        ProductId.Create(5),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(5)
                        }
                    ),

                    CouponOrderProduct.Create(
                        ProductId.Create(6),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    )
                ],
                100m
            ),
            false
        ],
        [
            CouponOrder.Create(
                [
                    CouponOrderProduct.Create(
                        ProductId.Create(2),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    ),
                    CouponOrderProduct.Create(
                        ProductId.Create(6),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    ),
                ],
                100m
            ),
            false
        ],
        [
            CouponOrder.Create(
                [
                    CouponOrderProduct.Create(
                        ProductId.Create(5),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    ),
                    CouponOrderProduct.Create(
                        ProductId.Create(3),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    ),
                ],
                200m
            ),
            true
        ],
        [
            CouponOrder.Create(
                [
                    CouponOrderProduct.Create(
                        ProductId.Create(1),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    ),
                    CouponOrderProduct.Create(
                        ProductId.Create(6),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    ),
                ],
                100m
            ),
            true
        ],
    ];

    /// <summary>
    /// Verifies the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method
    /// returns the expected boolean value.
    /// </summary>
    /// <param name="order">The order input.</param>
    /// <param name="expected">The expected result.</param>
    [Theory]
    [MemberData(nameof(CanOrderApplyCoupon))]
    public void CanBeApplied_WithRestrictionAndMinimumValue_ReturnsExpectedBoolean(
        CouponOrder order,
        bool expected
    )
    {
        var coupon = CouponUtils.CreateCoupon(
            minPrice: 10m,
            discount: DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                endingDate: DateTimeOffset.UtcNow.AddHours(5)
            ),
            usageLimit: 100
        );

        var productRestriction = CouponProductRestriction.Create([
            CouponProduct.Create(ProductId.Create(1)),
            CouponProduct.Create(ProductId.Create(2)),
            CouponProduct.Create(ProductId.Create(3)),
        ]);

        var categoryRestriction = CouponCategoryRestriction.Create(
            [CouponCategory.Create(CategoryId.Create(1))],
            [CouponProduct.Create(ProductId.Create(2))]
        );

        coupon.AssignRestriction(productRestriction);
        coupon.AssignRestriction(categoryRestriction);

        var result = coupon.CanBeApplied(order);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Verifies the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns
    /// false when the coupon is inactive.
    /// </summary>
    [Fact]
    public void CanBeApplied_WithInactiveCoupon_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(active: false);

        var result = coupon.CanBeApplied(CouponUtils.CreateCouponOrder());

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns
    /// false when usage limit is insufficient.
    /// </summary>
    [Fact]
    public void CanBeApplied_WithZeroUsageLimit_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 0);

        var result = coupon.CanBeApplied(CouponUtils.CreateCouponOrder());

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns
    /// false when usage the coupon discount is not valid to date.
    /// </summary>
    [Fact]
    public void CanBeApplied_WithDiscountNotValidToDate_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(discount:
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddDays(2),
                endingDate: DateTimeOffset.UtcNow.AddDays(4)
            )
        );

        var result = coupon.CanBeApplied(CouponUtils.CreateCouponOrder());

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies the <see cref="Coupon.ToggleActivation"/> method toggles
    /// the <see cref="Coupon.IsActive"/> property correctly.
    /// </summary>
    [Fact]
    public void ToggleActivation_WithActiveCoupon_TogglesActivationCorrectly()
    {
        var coupon = CouponUtils.CreateCoupon();

        coupon.ToggleActivation();
        coupon.IsActive.Should().BeFalse();

        coupon.ToggleActivation();
        coupon.IsActive.Should().BeTrue();
    }
}
