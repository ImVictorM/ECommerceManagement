using SharedKernel.ValueObjects;

namespace SharedKernel.Interfaces;

/// <summary>
/// Defines a contract for entities that can have discounts.
/// </summary>
public interface IDiscountable
{
    /// <summary>
    /// Gets the discounts.
    /// </summary>
    public IReadOnlyList<Discount> Discounts { get; }

    /// <summary>
    /// Gets the base price.
    /// </summary>
    public decimal BasePrice { get; }

    /// <summary>
    /// Adds discounts.
    /// </summary>
    /// <param name="discounts">The discounts to be added.</param>
    void AddDiscounts(params Discount[] discounts);

    /// <summary>
    /// Clears the discounts.
    /// </summary>
    void ClearDiscounts();

    /// <summary>
    /// Gets the price applying all the discounts.
    /// </summary>
    /// <returns>The price with discounts.</returns>
    decimal GetPriceAfterDiscounts();

    /// <summary>
    /// Gets the discounts that can be applied when calculating discounts.
    /// </summary>
    /// <returns>A list of discounts valid.</returns>
    IEnumerable<Discount> GetApplicableDiscounts();
}
