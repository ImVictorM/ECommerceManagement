using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.Extensions;

using FluentAssertions;

namespace Domain.UnitTests.CouponAggregate;

/// <summary>
/// Unit tests for the <see cref="Coupon"/> class.
/// </summary>
public class CouponTests
{
    /// <summary>
    /// List of valid actions to create coupons.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidActionsToCreateCoupons =
    [
        [
            () => CouponUtils.CreateCoupon(code: "code"),
        ],
        [
            () => CouponUtils.CreateCoupon(usageLimit: 15)
        ],
        [
            () => CouponUtils.CreateCoupon(minPrice: 2m)
        ],
        [
            () => CouponUtils.CreateCoupon(autoApply: false)
        ],
    ];

    /// <summary>
    /// Tests the coupon is created correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidActionsToCreateCoupons))]
    public void CreateCoupon_WithValidParameters_CreatesWithoutThrowing(
        Func<Coupon> action
    )
    {
        var actionResult = FluentActions
            .Invoking(action)
            .Should()
            .NotThrow();

        var coupon = actionResult.Subject;

        coupon.Discount.Should().NotBeNull();
        coupon.Code.Should().Be(coupon.Code.ToUpperSnakeCase());
        coupon.UsageLimit.Should().BePositive();
        coupon.MinPrice.Should().BePositive();
        coupon.IsActive.Should().BeTrue();
    }

    /// <summary>
    /// List containing coupon restrictions.
    /// </summary>
    public static readonly IEnumerable<object[]> CouponRestrictions =
    [
        [
            CategoryRestriction.Create(
                [
                    CouponCategory.Create(CategoryId.Create(1)),
                    CouponCategory.Create(CategoryId.Create(2))
                ]
            )
        ],
        [
            ProductRestriction.Create(
                [
                    CouponProduct.Create(ProductId.Create(1)),
                ]
            )
        ],
    ];

    /// <summary>
    /// Tests it is possible to assign different kind of restrictions to a coupon.
    /// </summary>
    /// <param name="restriction">The restriction.</param>
    [Theory]
    [MemberData(nameof(CouponRestrictions))]
    public void AssignRestriction_WhenAssigningDifferentKindOfRestrictions_AddsCorrectly(CouponRestriction restriction)
    {
        var coupon = CouponUtils.CreateCoupon();

        coupon.AssignRestriction(restriction);

        coupon.Restrictions.Should().Contain(restriction);
    }

    /// <summary>
    /// List with coupon orders and expected result when calling the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method for the test coupon.
    /// </summary>
    public static IEnumerable<object[]> CanOrderApplyCoupon =>
    [
        [
            CouponOrder.Create(
                [
                    (
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
                    (
                        ProductId.Create(5),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(5)
                        }
                    ),
                    (
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
                    (
                        ProductId.Create(2),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    ),
                    (
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
                    (
                        ProductId.Create(5),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    ),
                    (
                        ProductId.Create(3),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    )
                ],
                200m
            ),
            true
        ],
        [
            CouponOrder.Create(
                [
                    (
                        ProductId.Create(1),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(6)
                        }
                    ),
                    (
                        ProductId.Create(6),
                        new HashSet<CategoryId>()
                        {
                            CategoryId.Create(1)
                        }
                    )
                ],
                100m
            ),
            true
        ],
    ];

    /// <summary>
    /// Tests that the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns the expected valid for certain type of inputs.
    /// </summary>
    /// <param name="order">The order input.</param>
    /// <param name="expected">The expected result.</param>
    [Theory]
    [MemberData(nameof(CanOrderApplyCoupon))]
    public void CanBeApplied_WhenCheckingTheRestrictionsAndMinimumValue_ShouldReturnExpectedValue(
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

        var productRestriction = ProductRestriction.Create(
            [
                CouponProduct.Create(ProductId.Create(1)),
                CouponProduct.Create(ProductId.Create(2)),
                CouponProduct.Create(ProductId.Create(3)),
            ]
        );

        var categoryRestriction = CategoryRestriction.Create(
            [
                CouponCategory.Create(CategoryId.Create(1)),
            ],
            [
                CouponProduct.Create(ProductId.Create(2))
            ]
        );

        coupon.AssignRestriction(productRestriction);
        coupon.AssignRestriction(categoryRestriction);

        var result = coupon.CanBeApplied(order);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Tests the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns false when the coupon is inactive.
    /// </summary>
    [Fact]
    public void CanBeApplied_WhenCouponIsInactive_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(active: false);

        var result = coupon.CanBeApplied(CouponUtils.CreateCouponOrder());

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns false when usage limit is insufficient.
    /// </summary>
    [Fact]
    public void CanBeApplied_WhenUsageLimitIsNotSufficient_ReturnsFalse()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 0);

        var result = coupon.CanBeApplied(CouponUtils.CreateCouponOrder());

        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests the <see cref="Coupon.CanBeApplied(CouponOrder)"/> method returns false when usage the coupon discount is not valid to date.
    /// </summary>
    [Fact]
    public void CanBeApplied_WhenDiscountIsNotValidToDate_ReturnsFalse()
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
    /// Tests the <see cref="Coupon.Deactivate"/> method update the fields correct.
    /// </summary>
    [Fact]
    public void Deactivate_WhenCallingIt_UpdatesCouponCorrectly()
    {
        var coupon = CouponUtils.CreateCoupon(usageLimit: 500);

        coupon.Deactivate();

        coupon.IsActive.Should().BeFalse();
        coupon.UsageLimit.Should().Be(0);
    }
}
