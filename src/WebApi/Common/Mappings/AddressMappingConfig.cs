using Contracts.Common;

using SharedKernel.ValueObjects;

using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configures the mappings between address objects.
/// </summary>
public class AddressMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddressContract, Address>()
            .MapWith(src => Address.Create(
                src.PostalCode,
                src.Street,
                src.State,
                src.City,
                src.Neighborhood)
            );
    }
}
