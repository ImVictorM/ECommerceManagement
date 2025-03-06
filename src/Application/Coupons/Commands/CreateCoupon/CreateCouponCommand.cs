using Application.Common.DTOs;
using Application.Common.Security.Authorization.Requests;
using Application.Coupons.Abstracts;

using SharedKernel.ValueObjects;

namespace Application.Coupons.Commands.CreateCoupon;

/// <summary>
/// Represents a command to create a coupon.
/// </summary>
/// <param name="Discount">The coupon discount.</param>
/// <param name="Code">The coupon code.</param>
/// <param name="UsageLimit">The coupon usage limit.</param>
/// <param name="AutoApply">A boolean indicating if the coupon auto apply.</param>
/// <param name="MinPrice">The coupon minimum price.</param>
/// <param name="Restrictions">The coupon restrictions (Optional).</param>
[Authorize(roleName: nameof(Role.Admin))]
public record CreateCouponCommand(
    Discount Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<ICouponRestrictionInput>? Restrictions = null
) : IRequestWithAuthorization<CreatedResult>;

