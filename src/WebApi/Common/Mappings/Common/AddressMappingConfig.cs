using Contracts.Common;
using Mapster;
using SharedKernel.ValueObjects;

namespace WebApi.Common.Mappings.Common;

/// <summary>
/// Configures the mapping between contract address and domain address.
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
