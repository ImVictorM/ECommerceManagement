namespace Domain.CouponAggregate.Services;

/// <summary>
/// Provides services for coupon usage.
/// </summary>
public interface ICouponUsageService
{
    /// <summary>
    /// Determines whether the coupon still has remaining uses based on the number
    /// of non-canceled orders.
    /// </summary>
    /// <param name="coupon">The coupon to check.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// <c>true</c> if the coupon has remaining uses; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> IsWithinUsageLimitAsync(
        Coupon coupon,
        CancellationToken cancellationToken = default
    );
}
