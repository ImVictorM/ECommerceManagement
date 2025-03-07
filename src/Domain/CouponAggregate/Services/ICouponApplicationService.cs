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
    /// The collection of coupons applied ids.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// The total after coupon discounts have been applied.
    /// </returns>
    Task<decimal> ApplyCouponsAsync(
        CouponOrder order,
        IEnumerable<CouponId> couponToApplyIds,
        CancellationToken cancellationToken = default
    );
}
