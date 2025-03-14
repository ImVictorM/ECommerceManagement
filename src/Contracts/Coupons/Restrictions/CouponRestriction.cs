using System.Text.Json.Serialization;

namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a polymorphic coupon restriction.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(CouponProductRestriction), nameof(CouponProductRestriction))]
[JsonDerivedType(typeof(CouponCategoryRestriction), nameof(CouponCategoryRestriction))]
public abstract record CouponRestriction();
