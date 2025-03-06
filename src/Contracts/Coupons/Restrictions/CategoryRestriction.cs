namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a category coupon restriction contract.
/// </summary>
/// <param name="CategoryAllowedIds">
/// The category allowed ids.
/// </param>
/// <param name="ProductFromCategoryNotAllowedIds">
/// The product from the category ids that are not allowed.
/// </param>
public record CategoryRestriction(
    IEnumerable<string> CategoryAllowedIds,
    IEnumerable<string>? ProductFromCategoryNotAllowedIds = null
) : CouponRestriction();
