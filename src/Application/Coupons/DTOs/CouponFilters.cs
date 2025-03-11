namespace Application.Coupons.DTOs;

/// <summary>
/// Represents filtering criteria for coupon queries.
/// </summary>
/// <param name = "Active" >
/// Filters coupons by their activation status.
/// <para>true: Returns only active coupons.</para>
/// <para>false: Returns only inactive coupons.</para>
/// <para>null (default): Activation status is not considered.</para>
/// </param>
///  <param name="ExpiringAfter">
/// Filters coupons that expire after the specified UTC date.
/// </param>
/// <param name="ExpiringBefore">
/// Filters coupons that expire before the specified UTC date.
/// </param>
/// <param name="ValidForDate">
/// Filters coupons that were/will be valid during a specific UTC date.
/// </param>
public record CouponFilters(
    bool? Active = null,
    DateTimeOffset? ExpiringAfter = null,
    DateTimeOffset? ExpiringBefore = null,
    DateTimeOffset? ValidForDate = null
);
