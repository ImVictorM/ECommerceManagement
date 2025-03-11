using Application.Common.Security.Authorization.Requests;
using Application.Coupons.Abstracts;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Coupons.Commands.UpdateCoupon;

/// <summary>
/// Represents a command to update a coupon.
/// </summary>
/// <param name="CouponId">The coupon identifier.</param>
/// <param name="Discount">The new coupon discount.</param>
/// <param name="Code">The new coupon code.</param>
/// <param name="UsageLimit">The new coupon usage limit.</param>
/// <param name="AutoApply">
/// A boolean flag indicating if the coupon should auto apply.
/// </param>
/// <param name="MinPrice">The new coupon minimum price.</param>
/// <param name="Restrictions">The new coupon restrictions (optional).</param>
[Authorize(roleName: nameof(Role.Admin))]
public record UpdateCouponCommand(
    string CouponId,
    Discount Discount,
    string Code,
    int UsageLimit,
    bool AutoApply,
    decimal MinPrice,
    IEnumerable<ICouponRestrictionInput>? Restrictions = null
) : IRequestWithAuthorization<Unit>;
