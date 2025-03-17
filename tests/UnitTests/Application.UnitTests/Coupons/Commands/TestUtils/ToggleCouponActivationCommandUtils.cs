using Application.Coupons.Commands.ToggleCouponActivation;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Coupons.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="ToggleCouponActivationCommand"/> class.
/// </summary>
public static class ToggleCouponActivationCommandUtils
{
    /// <summary>
    /// Creates a new instance of the
    /// <see cref="ToggleCouponActivationCommand"/> class.
    /// </summary>
    /// <param name="couponId">The coupon identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="ToggleCouponActivationCommand"/> class.
    /// </returns>
    public static ToggleCouponActivationCommand CreateCommand(
        string? couponId = null
    )
    {
        return new ToggleCouponActivationCommand(
            couponId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
