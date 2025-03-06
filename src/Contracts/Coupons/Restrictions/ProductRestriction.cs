namespace Contracts.Coupons.Restrictions;

/// <summary>
/// Represents a product restriction contract.
/// </summary>
/// <param name="ProductAllowedIds">The product allowed ids.</param>
public record ProductRestriction(
    IEnumerable<string> ProductAllowedIds
) : CouponRestriction();
