using SharedKernel.ValueObjects;

namespace SharedKernel.Interfaces;

/// <summary>
/// Defines the contract for discount operations.
/// </summary>
public interface IDiscountService
{
    /// <summary>
    /// Calculates the price applying the discounts.
    /// The total is calculated recursively by ordering the discounts in
    /// descending order and applying them one by one.
    /// </summary>
    /// <param name="basePrice">The base price.</param>
    /// <param name="discountsToApply">The discounts.</param>
    /// <returns>The total applying the discounts.</returns>
    decimal CalculateDiscountedPrice(
        decimal basePrice,
        IEnumerable<Discount> discountsToApply
    );
}
