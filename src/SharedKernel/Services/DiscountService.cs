using SharedKernel.ValueObjects;

namespace SharedKernel.Services;

/// <summary>
/// Service to handle discount shared logic.
/// </summary>
public static class DiscountService
{
    /// <summary>
    /// Calculates the price applying the discounts.
    /// The total is calculated recursively by ordering the discount in descending order
    /// and applying them one by one.
    /// </summary>
    /// <param name="basePrice">The base price.</param>
    /// <param name="discountsToApply">The discounts.</param>
    /// <returns>The total applying the discounts.</returns>
    public static decimal ApplyDiscounts(decimal basePrice, params Discount[] discountsToApply)
    {
        return discountsToApply 
            .OrderByDescending(a => a.Percentage)
            .Aggregate(basePrice, (total, discount) => CalculateDiscount(total, discount.Percentage));
    }

    private static decimal CalculateDiscount(decimal amount, int percentage)
    {
        return amount - (amount * (percentage / 100m));
    }
}
