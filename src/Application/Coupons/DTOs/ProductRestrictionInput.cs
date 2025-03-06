using Application.Coupons.Abstracts;

namespace Application.Coupons.DTOs;

/// <summary>
/// Represents a product restriction input.
/// </summary>
/// <param name="ProductAllowedIds">The product allowed ids.</param>
public record ProductRestrictionInput(
    IEnumerable<string> ProductAllowedIds
) : ICouponRestrictionInput;
