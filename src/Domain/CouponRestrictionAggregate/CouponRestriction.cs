using Domain.CouponAggregate.ValueObjects;
using Domain.CouponRestrictionAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate;

/// <summary>
/// Represents a base coupon restriction.
/// </summary>
public abstract class CouponRestriction : AggregateRoot<CouponRestrictionId>
{
    /// <summary>
    /// Gets the coupon id.
    /// </summary>
    public CouponId CouponId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponRestriction"/> class.
    /// </summary>
    protected CouponRestriction() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponRestriction"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    protected CouponRestriction(CouponId couponId)
    {
        CouponId = couponId;
    }

    /// <summary>
    /// Determines if a certain order context passes the restriction constraints.
    /// </summary>
    /// <param name="order">The given order context.</param>
    /// <returns>A boolean value indicating if the context passes the restriction constraints.</returns>
    public abstract bool PassRestriction(CouponRestrictionOrder order);
}
