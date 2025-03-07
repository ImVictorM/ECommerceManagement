using Application.Common.Persistence.Repositories;
using Application.Orders.Errors;

using Domain.CouponAggregate;
using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;

using SharedKernel.Interfaces;

namespace Application.Coupons.Services;

internal sealed class CouponApplicationService : ICouponApplicationService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IDiscountService _discountService;

    public CouponApplicationService(
        ICouponRepository couponRepository,
        IDiscountService discountService
    )
    {
        _couponRepository = couponRepository;
        _discountService = discountService;
    }

    /// <inheritdoc/>
    public async Task<decimal> ApplyCouponsAsync(
        CouponOrder order,
        IEnumerable<CouponId> couponToApplyIds,
        CancellationToken cancellationToken = default
    )
    {
        var coupons = await _couponRepository.FindAllAsync(
            c => couponToApplyIds.Contains(c.Id),
            cancellationToken
        );

        ValidateCouponsApplication(coupons, couponToApplyIds, order);

        var couponDiscounts = coupons.Select(c => c.Discount);

        return _discountService.CalculateDiscountedPrice(
            order.Total,
            couponDiscounts
        );
    }

    private static void ValidateCouponsApplication(
        IEnumerable<Coupon> coupons,
        IEnumerable<CouponId> couponToApplyIds,
        CouponOrder order)
    {
        var couponsMap = coupons.ToDictionary(c => c.Id);

        foreach (var couponId in couponToApplyIds)
        {
            if (!couponsMap.TryGetValue(couponId, out var coupon))
            {
                throw new InvalidCouponAppliedException(
                    $"The coupon with id {couponId} is expired or invalid"
                );
            }

            if (!coupon.CanBeApplied(order))
            {
                throw new InvalidCouponAppliedException(
                    $"The coupon with id {coupon.Id} cannot be applied because" +
                    $" the order does not meet the requirements"
                );
            }
        }
    }
}
