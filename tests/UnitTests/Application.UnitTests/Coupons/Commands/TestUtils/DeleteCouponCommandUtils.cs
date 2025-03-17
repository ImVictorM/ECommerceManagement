using Application.Coupons.Commands.DeleteCoupon;

using Domain.UnitTests.TestUtils;

namespace Application.UnitTests.Coupons.Commands.TestUtils;

/// <summary>
/// Utilities for the <see cref="DeleteCouponCommand"/> class.
/// </summary>
public static class DeleteCouponCommandUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="DeleteCouponCommand"/> class.
    /// </summary>
    /// <param name="couponId">The coupon identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="DeleteCouponCommand"/> class.
    /// </returns>
    public static DeleteCouponCommand CreateCommand(
        string? couponId = null
    )
    {
        return new DeleteCouponCommand(
            couponId ?? NumberUtils.CreateRandomLongAsString()
        );
    }
}
