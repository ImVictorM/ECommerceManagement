using System.Text.Json.Serialization;

namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a polymorphic coupon restriction.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(ProductRestriction), nameof(ProductRestriction))]
[JsonDerivedType(typeof(CategoryRestriction), nameof(CategoryRestriction))]
public abstract record CouponRestriction();
