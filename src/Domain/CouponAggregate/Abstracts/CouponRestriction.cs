using Domain.CouponAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponAggregate.Abstracts;

/// <summary>
/// Represents a base coupon restriction.
/// </summary>
public abstract class CouponRestriction : ValueObject
{

    /// <summary>
    /// Initiates a new instance of the <see cref="CouponRestriction"/> class.
    /// </summary>
    protected CouponRestriction() { }

    /// <summary>
    /// Determines if a certain order context passes the restriction constraints.
    /// </summary>
    /// <param name="order">The given order context.</param>
    /// <returns>A boolean value indicating if the context passes the restriction constraints.</returns>
    public abstract bool PassRestriction(CouponOrder order);
}
