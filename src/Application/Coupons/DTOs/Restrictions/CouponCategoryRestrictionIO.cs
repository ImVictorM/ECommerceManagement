namespace Application.Coupons.DTOs.Restrictions;

/// <summary>
/// Represents a category restriction input.
/// </summary>
/// <param name="CategoryAllowedIds">
/// The category allowed identifiers.
/// </param>
/// <param name="ProductFromCategoryNotAllowedIds">
/// The product from the category ids that are not allowed.
/// </param>
public record CouponCategoryRestrictionIO(
    IEnumerable<string> CategoryAllowedIds,
    IEnumerable<string>? ProductFromCategoryNotAllowedIds = null
) : CouponRestrictionIO();
