using Mapster;
using DomainDiscount = SharedKernel.ValueObjects.Discount;
using ContractDiscount = Contracts.Products.Common.Discount;

namespace WebApi.Common.Mappings.Common;

/// <summary>
/// Configure mapping between discount objects.
/// </summary>
public class DiscountMappingConfig : IRegister
{
    /// <summary>
    /// Register mapping for discount objects.
    /// </summary>
    /// <param name="config">The global configuration object.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ContractDiscount, DomainDiscount>()
            .MapWith(src => DomainDiscount.Create(
                src.Percentage,
                src.Description,
                src.StartingDate,
                src.EndingDate));
    }
}
