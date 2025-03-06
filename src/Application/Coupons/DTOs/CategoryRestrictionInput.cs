using Application.Coupons.Abstracts;

namespace Application.Coupons.DTOs;

/// <summary>
/// Represents a category restriction input.
/// </summary>
/// <param name="CategoryAllowedIds">
/// The category allowed ids.
/// </param>
/// <param name="ProductFromCategoryNotAllowedIds">
/// The product from the category ids that are not allowed.
/// </param>
public record CategoryRestrictionInput(
    IEnumerable<string> CategoryAllowedIds,
    IEnumerable<string>? ProductFromCategoryNotAllowedIds = null
) : ICouponRestrictionInput;
