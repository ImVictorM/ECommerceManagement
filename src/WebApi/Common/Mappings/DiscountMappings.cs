using SharedKernel.ValueObjects;

using Contracts.Common;

using Mapster;

namespace WebApi.Common.Mappings;

internal sealed class DiscountMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<DiscountContract, Discount>()
            .MapWith(src => Discount.Create(
                Percentage.Create(src.Percentage),
                src.Description,
                src.StartingDate,
                src.EndingDate
            ));

        config.NewConfig<Discount, DiscountContract>();
    }
}
