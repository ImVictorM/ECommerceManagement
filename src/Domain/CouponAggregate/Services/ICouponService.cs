using Domain.CouponAggregate.ValueObjects;

namespace Domain.CouponAggregate.Services;

/// <summary>
/// Provides services for coupons.
/// </summary>
public interface ICouponService
{
    /// <summary>
    /// Determines whether the specified coupon can be applied to the given order
    /// checking its restriction and invariants.
    /// </summary>
    /// <param name="coupon">The current coupon.</param>
    /// <param name="couponOrder">The details of the order to evaluate.</param>
    /// <returns>A boolean value indicating whether the coupon is applicable to the order.</returns>
    Task<bool> IsApplicableAsync(Coupon coupon, CouponOrder couponOrder);
}
