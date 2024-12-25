using Mapster;
using SharedKernel.ValueObjects;
using Contracts.Common;

namespace WebApi.Common.Mappings.Common;

/// <summary>
/// Configures mapping between discount objects.
/// </summary>
public class DiscountMappingConfig : IRegister
{
   /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<DiscountContract, Discount>()
            .MapWith(src => Discount.Create(
                Percentage.Create(src.Percentage),
                src.Description,
                src.StartingDate,
                src.EndingDate));
    }
}
