using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace SharedKernel.Services;

internal sealed class DiscountService : IDiscountService
{
    public decimal CalculateDiscountedPrice(
        decimal basePrice,
        IEnumerable<Discount> discountsToApply
    )
    {
        return discountsToApply
            .OrderByDescending(a => a.Percentage)
            .Aggregate(basePrice, (total, discount) =>
                CalculateDiscount(total, discount.Percentage.Value)
            );
    }

    private static decimal CalculateDiscount(decimal amount, int percentage)
    {
        return amount - (amount * (percentage / 100m));
    }
}
