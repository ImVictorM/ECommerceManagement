using Contracts.Common;
using Contracts.Coupons.Restrictions;

namespace Contracts.Coupons;

/// <summary>
/// Represents a request to update a coupon.
/// </summary>
/// <param name="Discount">The new coupon discount.</param>
/// <param name="Code">The new coupon code.</param>
/// <param name="UsageLimit">The new coupon usage limit.</param>
/// <param name="AutoApply">
/// A boolean flag indicating if the coupon should auto apply.
/// </param>
/// <param name="MinPrice">The new coupon minimum price.</param>
/// <param name="Restrictions">The new coupon restrictions (optional).</param>
public record UpdateCouponRequest(
    DiscountContract Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<CouponRestriction>? Restrictions = null
);
