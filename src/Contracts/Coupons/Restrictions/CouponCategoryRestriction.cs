namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a category coupon restriction.
/// </summary>
/// <param name="CategoryAllowedIds">
/// The category allowed identifiers.
/// </param>
/// <param name="ProductFromCategoryNotAllowedIds">
/// The product identifiers belonging to the category that are not allowed.
/// </param>
public record CouponCategoryRestriction(
    IEnumerable<string> CategoryAllowedIds,
    IEnumerable<string>? ProductFromCategoryNotAllowedIds = null
) : CouponRestriction();
