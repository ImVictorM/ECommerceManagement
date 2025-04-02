using Application.Common.Persistence.Repositories;
using Application.Coupons.Errors;

using Domain.CouponAggregate;
using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Coupons.Services;

internal sealed class CouponApplicationService : ICouponApplicationService
{
    private readonly ICouponUsageService _couponUsageService;
    private readonly ICouponRepository _couponRepository;
    private readonly IDiscountService _discountService;

    public CouponApplicationService(
        ICouponUsageService couponUsageService,
        ICouponRepository couponRepository,
        IDiscountService discountService
    )
    {
        _couponUsageService = couponUsageService;
        _couponRepository = couponRepository;
        _discountService = discountService;
    }

    public async Task<decimal> ApplyCouponsAsync(
        CouponOrder order,
        IEnumerable<CouponId> couponToApplyIds,
        CancellationToken cancellationToken = default
    )
    {
        var coupons = await _couponRepository.GetCouponsByIdsAsync(
            couponToApplyIds,
            cancellationToken
        );

        await ValidateCouponsApplication(
            coupons,
            couponToApplyIds,
            order,
            cancellationToken
        );

        var couponDiscounts = coupons.Select(c => c.Discount);

        return _discountService.CalculateDiscountedPrice(
            order.Total,
            couponDiscounts
        );
    }

    public async Task<bool> IsCouponApplicableAsync(
        Coupon coupon,
        CouponOrder order,
        CancellationToken cancellationToken = default
    )
    {
        return coupon.IsActive
            && coupon.Restrictions.All(r => r.PassRestriction(order))
            && coupon.Discount.IsValidToDate
            && order.Total >= coupon.MinPrice
            && await _couponUsageService.IsWithinUsageLimitAsync(
                coupon,
                cancellationToken);
    }

    private async Task ValidateCouponsApplication(
        IEnumerable<Coupon> coupons,
        IEnumerable<CouponId> couponToApplyIds,
        CouponOrder order,
        CancellationToken cancellationToken = default
    )
    {
        var couponsMap = coupons.ToDictionary(c => c.Id);

        foreach (var couponId in couponToApplyIds)
        {
            if (!couponsMap.TryGetValue(couponId, out var coupon))
            {
                throw new CouponNotFoundException();
            }

            if (!await IsCouponApplicableAsync(coupon, order, cancellationToken))
            {
                throw new CouponApplicationFailedException();
            }
        }
    }
}
