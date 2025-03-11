using Contracts.Common;
using Contracts.Coupons.Restrictions;

namespace Contracts.Coupons;

/// <summary>
/// Represents a coupon response.
/// </summary>
/// <param name="Id">The coupon identifier.</param>
/// <param name="Code">The coupon code.</param>
/// <param name="Discount">The coupon discount.</param>
/// <param name="UsageLimit">The coupon usage limit.</param>
/// <param name="AutoApply">The coupon auto apply flag.</param>
/// <param name="MinPrice">The coupon minimum price.</param>
/// <param name="Restrictions">The coupon restrictions.</param>
public record CouponResponse(
    string Id,
    string Code,
    DiscountContract Discount,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<CouponRestriction> Restrictions
);
