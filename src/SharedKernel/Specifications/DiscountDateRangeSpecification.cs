using SharedKernel.Interfaces;
using SharedKernel.Models;

namespace SharedKernel.Specifications;

public class DiscountDateRangeSpecification : CompositeSpecification<IDiscount>
{
    public override bool IsSatisfiedBy(IDiscount discount)
    {
        var now = DateTimeOffset.UtcNow;

        return discount.StartingDate > now.AddDays(-1) && discount.EndingDate > discount.StartingDate.AddHours(1);
    }
}
