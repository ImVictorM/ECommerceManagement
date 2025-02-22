using Domain.CouponAggregate;
using Domain.CouponAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for coupon persistence operations.
/// </summary>
public interface ICouponRepository : IBaseRepository<Coupon, CouponId>
{
}
