using Application.Coupons.DTOs.Restrictions;
using Application.Coupons.Extensions;

using Domain.CouponAggregate;

using SharedKernel.ValueObjects;

namespace Application.Coupons.DTOs.Results;

/// <summary>
/// Represents a coupon result.
/// </summary>
public class CouponResult
{
    /// <summary>
    /// Gets the coupon identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the coupon discount.
    /// </summary>
    public Discount Discount { get; }

    /// <summary>
    /// Gets the coupon code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the coupon usage limit.
    /// </summary>
    public int UsageLimit { get; }

    /// <summary>
    /// Gets the coupon auto apply boolean value.
    /// </summary>
    public bool AutoApply { get; }

    /// <summary>
    /// Gets the coupon minimum price.
    /// </summary>
    public decimal MinPrice { get; }

    /// <summary>
    /// Gets the coupon restrictions.
    /// </summary>
    public IReadOnlyList<CouponRestrictionIO> Restrictions { get; }

    internal CouponResult(Coupon coupon)
    {
        Id = coupon.Id.ToString();
        Discount = coupon.Discount;
        Code = coupon.Code;
        UsageLimit = coupon.UsageLimit;
        AutoApply = coupon.AutoApply;
        MinPrice = coupon.MinPrice;
        Restrictions = coupon.Restrictions.ParseRestrictions().ToList();
    }
}
