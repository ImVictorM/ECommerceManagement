using Application.Common.Persistence.Repositories;

using Domain.CouponAggregate;
using Domain.CouponAggregate.Services;

namespace Application.Coupons.Services;

internal sealed class CouponUsageService : ICouponUsageService
{
    private readonly ICouponRepository _couponRepository;

    public CouponUsageService(
        ICouponRepository couponRepository
    )
    {
        _couponRepository = couponRepository;
    }

    public async Task<bool> IsWithinUsageLimitAsync(
        Coupon coupon,
        CancellationToken cancellationToken = default
    )
    {
        var couponUsageCount = await _couponRepository.GetCouponUsageCountAsync(
            coupon.Id,
            cancellationToken
        );

        return couponUsageCount < coupon.UsageLimit;
    }
}
