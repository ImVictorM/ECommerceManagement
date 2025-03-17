using Contracts.Common;
using Contracts.Coupons.Restrictions;

namespace Contracts.Coupons;

/// <summary>
/// Represents a request to create a coupon.
/// </summary>
/// <param name="Discount">The coupon discount.</param>
/// <param name="Code">The coupon code.</param>
/// <param name="UsageLimit">The coupon usage limit.</param>
/// <param name="AutoApply">
/// A boolean indicating if the coupon should auto apply.
/// </param>
/// <param name="MinPrice">The coupon minimum price.</param>
/// <param name="Restrictions">The coupon restrictions (optional).</param>
public record CreateCouponRequest(
    DiscountContract Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<CouponRestriction>? Restrictions = null
);
