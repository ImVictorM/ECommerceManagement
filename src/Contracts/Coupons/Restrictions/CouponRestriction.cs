using System.Text.Json.Serialization;

namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a polymorphic coupon restriction.
/// </summary>
[JsonPolymorphic]
[JsonDerivedType(typeof(CategoryRestriction))]
[JsonDerivedType(typeof(ProductRestriction))]
public abstract record CouponRestriction();
