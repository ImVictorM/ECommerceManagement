using SharedKernel.ValueObjects;

using Contracts.Common;

using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configures the mappings between discount objects.
/// </summary>
public class DiscountMappings : IRegister
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
