using Domain.CouponAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an order coupon.
/// </summary>
public class OrderCoupon : ValueObject
{
    /// <summary>
    /// Gets the coupon id.
    /// </summary>
    public CouponId CouponId { get; } = null!;

    private OrderCoupon() { }

    private OrderCoupon(CouponId couponId)
    {
        CouponId = couponId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderCoupon"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    /// <returns>A new instance of the <see cref="OrderCoupon"/> class.</returns>
    public static OrderCoupon Create(CouponId couponId)
    {
        return new OrderCoupon(couponId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CouponId;
    }
}
