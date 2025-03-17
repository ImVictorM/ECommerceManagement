using Domain.CategoryAggregate.ValueObjects;
using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.CouponAggregate.Abstracts;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Bogus;

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
    /// <param name="minPrice">
    /// The coupon minimum price. The default value is 0.
    /// </param>
    /// <param name="autoApply">The coupon auto apply flag.</param>
    /// <param name="active">The initial state of the coupon.</param>
    /// <param name="initialRestrictions">
    /// The coupon initial restrictions.
    /// </param>
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
            discount ?? DiscountUtils.CreateDiscountValidToDate(),
            code ?? _faker.Lorem.Word(),
            usageLimit ?? _faker.Random.Int(50, 100),
            minPrice ?? 0m,
            autoApply ?? _faker.Random.Bool()
        );

        if (id != null)
        {
            coupon.SetIdUsingReflection(id);
        }

        if (!active)
        {
            coupon.ToggleActivation();
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
    /// Creates a collection of <see cref="Coupon"/>.
    /// </summary>
    /// <param name="count">The quantity of coupons to be created.</param>
    /// <returns>A collection of <see cref="Coupon"/>.</returns>
    public static IReadOnlyCollection<Coupon> CreateCoupons(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateCoupon(
                id: CouponId.Create(index + 1)
            ))
            .ToList();
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CouponOrder"/> class.
    /// </summary>
    /// <param name="products">The order products.</param>
    /// <param name="total">The order total.</param>
    /// <returns>A new instance of the <see cref="CouponOrder"/> class.</returns>
    public static CouponOrder CreateCouponOrder(
        HashSet<CouponOrderProduct>? products = null,
        decimal? total = null
    )
    {
        HashSet<CouponOrderProduct> productsDefault =
        [
            CouponOrderProduct.Create(
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
