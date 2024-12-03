using SharedKernel.Interfaces;

namespace Domain.ProductAggregate.Specifications;

/// <summary>
/// Specification that defines a threshold for a product object.
/// Total discount must not exceed 90% of the base price.
/// </summary>
public class DiscountThresholdSpecification : ISpecification<Product>
{
    /// <inheritdoc/>
    public bool IsSatisfiedBy(Product entity)
    {
        var minimumPriceAfterDiscount = entity.BasePrice - entity.BasePrice * 0.9m;

        return entity.GetPriceAfterDiscounts() > minimumPriceAfterDiscount;
    }
}
