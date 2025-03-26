using Domain.CouponAggregate.ValueObjects;

namespace Domain.CouponAggregate.Services;

/// <summary>
/// Provides services for applying coupon discounts.
/// </summary>
public interface ICouponApplicationService
{
    /// <summary>
    /// Applies the provided coupons to the base total.
    /// </summary>
    /// <param name="order">The coupon order.</param>
    /// <param name="couponToApplyIds">
    /// The collection of coupons applied identifiers.
    /// </param>
    /// <param name="cancellationToken">
    /// The cancellation token.
    /// </param>
    /// <returns>
    /// The total after coupon discounts have been applied.
    /// </returns>
    Task<decimal> ApplyCouponsAsync(
        CouponOrder order,
        IEnumerable<CouponId> couponToApplyIds,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Checks if a coupon can be applied based on the given order context.
    /// </summary>
    /// <param name="coupon">The coupon to be checked.</param>
    /// <param name="order">The coupon order.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// <c>true</c> if the coupon can be applied; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> IsCouponApplicableAsync(
        Coupon coupon,
        CouponOrder order,
        CancellationToken cancellationToken = default
    );
}
