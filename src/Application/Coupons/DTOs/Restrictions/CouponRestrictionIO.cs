using System.Text.Json.Serialization;

namespace Application.Coupons.DTOs.Restrictions;

/// <summary>
/// Represents a polymorphic input/output for coupon restrictions.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(CouponProductRestrictionIO), nameof(CouponProductRestrictionIO))]
[JsonDerivedType(typeof(CouponCategoryRestrictionIO), nameof(CouponCategoryRestrictionIO))]
public abstract record CouponRestrictionIO;
