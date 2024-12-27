using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.Abstracts;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponAggregate.ValueObjects.Restrictions;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.CouponAggregate;

/// <summary>
/// Unit tests for the <see cref="Coupon"/> class.
/// </summary>
public class CouponTests
{
    /// <summary>
    /// List of valid coupon creation parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidCouponCreationParameters =
    [
        [
            DiscountUtils.CreateDiscount(PercentageUtils.Create(DomainConstants.Coupon.DiscountPercentage)),
            DomainConstants.Coupon.Code,
            DomainConstants.Coupon.UsageLimit,
            DomainConstants.Coupon.MinPrice,
            DomainConstants.Coupon.AutoApply
        ],
        [
            DiscountUtils.CreateDiscount(PercentageUtils.Create(6)),
            "CODE",
            15,
            2m,
            false
        ],
    ];

    /// <summary>
    /// Tests the coupon is created correctly.
    /// </summary>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">The coupon minimum price.</param>
    /// <param name="autoApply">The coupon auto apply flag.</param>
    [Theory]
    [MemberData(nameof(ValidCouponCreationParameters))]
    public void CreateCoupon_WhenCreatingWithValidParameters_ReturnsInstance(
        Discount discount,
        string code,
        int usageLimit,
        decimal minPrice,
        bool autoApply
    )
    {
        var coupon = CouponUtils.CreateCoupon(
            discount: discount,
            code: code,
            usageLimit: usageLimit,
            minPrice: minPrice,
            autoApply: autoApply
        );

        coupon.Discount.Should().BeEquivalentTo(discount);
        coupon.Code.Should().BeEquivalentTo(code);
        coupon.UsageLimit.Should().Be(usageLimit);
        coupon.MinPrice.Should().Be(minPrice);
        coupon.AutoApply.Should().Be(autoApply);
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
        var coupon = CouponUtils.CreateCoupon(minPrice: 10m);

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
