using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

public class DiscountPercentageRangeSpecification : CompositeSpecification<IDiscount>
{
    public override bool IsSatisfiedBy(IDiscount discount)
    {
        return discount.Percentage >= 0 && discount.Percentage <= 100;
    }
    //throw new DomainValidationException("Discount percentage must be between 1 and 100");
}
