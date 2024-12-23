using System.Linq.Expressions;
using Domain.CouponAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.CouponRestrictionAggregate.Specifications;

/// <summary>
/// Specification to query restrictions for a coupon.
/// </summary>
public class QueryCouponRestrictionByCouponIdSpecification : CompositeQuerySpecification<CouponRestriction>
{
    private readonly CouponId _couponId;

    /// <summary>
    /// Initiates a new instance of the <see cref="QueryCouponRestrictionByCouponIdSpecification"/> class.
    /// </summary>
    /// <param name="couponId">The coupon id.</param>
    public QueryCouponRestrictionByCouponIdSpecification(CouponId couponId)
    {
        _couponId = couponId;
    }

    /// <inheritdoc/>
    public override Expression<Func<CouponRestriction, bool>> Criteria => restriction => restriction.CouponId == _couponId;
}
