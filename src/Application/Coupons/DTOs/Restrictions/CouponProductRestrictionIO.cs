namespace Application.Coupons.DTOs.Restrictions;

/// <summary>
/// Represents a product restriction input.
/// </summary>
/// <param name="ProductAllowedIds">The product allowed identifiers.</param>
public record CouponProductRestrictionIO(
    IEnumerable<string> ProductAllowedIds
) : CouponRestrictionIO();
