using SharedKernel.Models;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification that defines a threshold for a product object.
/// Total discount must not exceed 90% of the base price.
/// </summary>
public class DiscountThresholdSpecification : CompositeSpecification<Product>
{
    /// <inheritdoc/>
    public override bool IsSatisfiedBy(Product entity)
    {
        var minimumPriceAfterDiscount = entity.BasePrice - entity.BasePrice * 0.9m;

        return entity.GetPriceAfterDiscounts() > minimumPriceAfterDiscount;
    }
}
