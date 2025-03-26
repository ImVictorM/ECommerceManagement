using Application.Common.Security.Authorization.Requests;
using Application.Coupons.DTOs.Restrictions;

using SharedKernel.ValueObjects;
using Application.Common.DTOs.Results;

namespace Application.Coupons.Commands.CreateCoupon;

/// <summary>
/// Represents a command to create a coupon.
/// </summary>
/// <param name="Discount">The coupon discount.</param>
/// <param name="Code">The coupon code.</param>
/// <param name="UsageLimit">The coupon usage limit.</param>
/// <param name="AutoApply">A boolean indicating if the coupon auto apply.</param>
/// <param name="MinPrice">The coupon minimum price.</param>
/// <param name="Restrictions">The coupon restrictions (optional).</param>
[Authorize(roleName: nameof(Role.Admin))]
public record CreateCouponCommand(
    Discount Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<CouponRestrictionIO>? Restrictions = null
) : IRequestWithAuthorization<CreatedResult>;

