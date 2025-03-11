using Application.Common.Security.Authorization.Requests;

using SharedKernel.ValueObjects;

using MediatR;

namespace Application.Coupons.Commands.ToggleCouponActivation;

/// <summary>
/// Represents a command to toggle a coupon's activation.
/// </summary>
/// <param name="CouponId">The coupon identifier.</param>
[Authorize(roleName: nameof(Role.Admin))]
public record ToggleCouponActivationCommand(string CouponId)
    : IRequestWithAuthorization<Unit>;
