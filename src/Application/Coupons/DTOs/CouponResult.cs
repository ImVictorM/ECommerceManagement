using Application.Coupons.DTOs.Restrictions;

using SharedKernel.ValueObjects;

namespace Application.Coupons.DTOs;

/// <summary>
/// Represents a coupon response.
/// </summary>
/// <param name="Id">The coupon id.</param>
/// <param name="Discount">The coupon discount.</param>
/// <param name="Code">The coupon code.</param>
/// <param name="UsageLimit">The coupon usage limit.</param>
/// <param name="AutoApply">The coupon auto apply value.</param>
/// <param name="MinPrice">The coupon minimum price.</param>
/// <param name="Restrictions">The coupon restrictions.</param>
public record CouponResult(
    string Id,
    Discount Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<CouponRestrictionIO> Restrictions
);
