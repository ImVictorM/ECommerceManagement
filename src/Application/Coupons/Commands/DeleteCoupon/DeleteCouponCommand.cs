using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Coupons.Commands.DeleteCoupon;

/// <summary>
/// Represents a command to delete a coupon.
/// </summary>
/// <param name="CouponId">The coupon identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record DeleteCouponCommand(string CouponId) : IRequestWithAuthorization<Unit>;
