namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a product restriction contract.
/// </summary>
/// <param name="ProductAllowedIds">The product allowed identifiers.</param>
public record CouponProductRestriction(
    IEnumerable<string> ProductAllowedIds
) : CouponRestriction();
