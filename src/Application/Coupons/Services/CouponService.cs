using Application.Common.Interfaces.Persistence;
using Domain.CouponAggregate;
using Domain.CouponAggregate.Services;
using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.Specifications;
using Domain.CouponRestrictionAggregate.ValueObjects;

namespace Application.Coupons.Services;

/// <summary>
/// Coupon services.
/// </summary>
public class CouponService : ICouponService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public CouponService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task<bool> IsApplicableAsync(Coupon coupon, CouponOrder couponOrder)
    {
        var couponRestrictions = await _unitOfWork.CouponRestrictionRepository.FindSatisfyingAsync(
            new QueryCouponRestrictionByCouponIdSpecification(coupon.Id)
        );

        var orderProducts = couponOrder.Products.Select(p => CouponRestrictionOrderProduct.Create(p.ProductId, p.CategoryIds));

        var restrictionsPass = couponRestrictions.All(restriction => restriction.PassRestriction(CouponRestrictionOrder.Create(orderProducts)));

        return restrictionsPass
            && coupon.IsActive
            && coupon.Discount.IsValidToDate
            && couponOrder.Total >= coupon.MinPrice
            && coupon.UsageLimit > 0;
    }
}
