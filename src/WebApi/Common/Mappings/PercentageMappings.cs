using SharedKernel.ValueObjects;

using Mapster;

namespace WebApi.Common.Mappings;

internal sealed class PercentageMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<int, Percentage>()
            .MapWith(src => Percentage.Create(src));

        config
            .NewConfig<Percentage, int>()
            .MapWith(src => src.Value);
    }
}
