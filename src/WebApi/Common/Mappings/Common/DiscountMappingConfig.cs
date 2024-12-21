using Mapster;
using DomainDiscount = SharedKernel.ValueObjects.Discount;
using ContractDiscount = Contracts.Common.Discount;
using SharedKernel.ValueObjects;

namespace WebApi.Common.Mappings.Common;

/// <summary>
/// Configures mapping between discount objects.
/// </summary>
public class DiscountMappingConfig : IRegister
{
   /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ContractDiscount, DomainDiscount>()
            .MapWith(src => DomainDiscount.Create(
                Percentage.Create(src.Percentage),
                src.Description,
                src.StartingDate,
                src.EndingDate));
    }
}
