using Application.Coupons.DTOs;

using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

namespace Application.Common.Persistence.Repositories;

/// <summary>
/// Defines the contract for coupon persistence operations.
/// </summary>
public interface ICouponRepository : IBaseRepository<Coupon, CouponId>
{
    /// <summary>
    /// Retrieves a collection of coupons based on a list of coupon ids.
    /// </summary>
    /// <param name="couponIds">The list of coupon ids.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// An <see cref="IEnumerable{Coupon}"/> of coupons that match the provided
    /// ids.
    /// </returns>
    Task<IEnumerable<Coupon>> GetCouponsByIdsAsync(
        IEnumerable<CouponId> couponIds,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Retrieves a collection filtered coupons.
    /// </summary>
    /// <param name="filters">The filter to be applied.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// An <see cref="IEnumerable{Coupon}"/> of coupons that match the provided
    /// filters.
    /// </returns>
    Task<IEnumerable<Coupon>> GetCouponsAsync(
        CouponFilters filters,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Calculates the number of times a coupon has already been used.
    /// </summary>
    /// <param name="couponId">The coupon identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    /// An integer representing the number of times the coupon has already been used.
    /// </returns>
    Task<int> GetCouponUsageCountAsync(
        CouponId couponId,
        CancellationToken cancellationToken = default
    );
}
