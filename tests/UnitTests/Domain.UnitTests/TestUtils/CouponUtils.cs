using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Bogus;
using Domain.CouponAggregate.Abstracts;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Coupon"/> class.
/// </summary>
public static class CouponUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Coupon"/> class.
    /// </summary>
    /// <param name="id">The coupon id.</param>
    /// <param name="discount">The coupon discount.</param>
    /// <param name="code">The coupon code.</param>
    /// <param name="usageLimit">The coupon usage limit.</param>
    /// <param name="minPrice">The coupon minimum price.</param>
    /// <param name="autoApply">The coupon auto apply flag.</param>
    /// <param name="active">The initial state of the coupon.</param>
    /// <param name="initialRestrictions">The coupon initial restrictions.</param>
    /// <returns>A new instance of the <see cref="Coupon"/> class.</returns>
    public static Coupon CreateCoupon(
        CouponId? id = null,
        Discount? discount = null,
        string? code = null,
        int? usageLimit = null,
        decimal? minPrice = null,
        bool? autoApply = null,
        bool active = true,
        IEnumerable<CouponRestriction>? initialRestrictions = null
    )
    {
        var coupon = Coupon.Create(
            discount ?? DiscountUtils.CreateDiscount(),
            code ?? _faker.Lorem.Word(),
            usageLimit ?? _faker.Random.Int(0, 100),
            minPrice ?? _faker.Finance.Amount(0, 1000),
            autoApply ?? _faker.Random.Bool()
        );

        if (id != null)
        {
            coupon.SetIdUsingReflection(id);
        }

        if (!active)
        {
            coupon.Deactivate();
        }

        if (initialRestrictions != null)
        {
            foreach (var restriction in initialRestrictions)
            {

                coupon.AssignRestriction(restriction);
            }
        }

        return coupon;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrder"/> class.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="CouponOrder"/> class.</returns>
    public static CouponOrder CreateCouponOrder(
        HashSet<(ProductId ProductId, IReadOnlySet<CategoryId> Categories)>? products = null,
        decimal? total = null
    )
    {
        HashSet<(ProductId ProductId, IReadOnlySet<CategoryId> Categories)> productsDefault =
        [
            (
                ProductId.Create(_faker.Random.Int(1, 100)),
                new HashSet<CategoryId>
                {
                    CategoryId.Create(2)
                }
            )
        ];

        return CouponOrder.Create(
            products ?? productsDefault,
            total ?? _faker.Finance.Amount(50, 5000)
        );
    }
}
