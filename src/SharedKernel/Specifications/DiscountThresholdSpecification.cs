using SharedKernel.Interfaces;

namespace SharedKernel.Specifications;

/// <summary>
/// Specification that defines a threshold for a discountable object
/// to no discount more than 90% of the discountable base price.
/// </summary>
public class DiscountThresholdSpecification : ISpecification<IDiscountable>
{
    /// <inheritdoc/>
    public bool IsSatisfiedBy(IDiscountable entity)
    {
        var minimumPriceAfterDiscount = entity.BasePrice - (entity.BasePrice * 0.9m);

        return entity.GetPriceAfterDiscounts() > minimumPriceAfterDiscount;
    }
}
