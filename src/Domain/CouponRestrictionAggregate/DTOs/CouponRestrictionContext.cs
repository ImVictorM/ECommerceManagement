namespace Domain.CouponRestrictionAggregate.DTOs;

/// <summary>
/// Represents an input to check if the restriction is valid for the given context.
/// </summary>
/// <param name="Products">The context products.</param>
public record CouponRestrictionContext(
    IEnumerable<ContextProduct> Products
);

