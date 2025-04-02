using Application.Coupons.DTOs.Filters;
using Application.Coupons.Queries.GetCoupons;

namespace Application.UnitTests.Coupons.Queries.TestUtils;

/// <summary>
/// Utilities for the <see cref="GetCouponsQuery"/> class.
/// </summary>
public static class GetCouponsQueryUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="GetCouponsQuery"/> class.
    /// </summary>
    /// <param name="filters">The filters.</param>
    /// <returns>
    /// A new instance of the <see cref="GetCouponsQuery"/> class.
    /// </returns>
    public static GetCouponsQuery CreateQuery(
        CouponFilters? filters = null
    )
    {
        return new GetCouponsQuery(
            filters ?? new CouponFilters()
        );
    }
}
