using Mapster;

using ContractAddress = Contracts.Common.Address;
using DomainAddress = SharedKernel.ValueObjects.Address;

namespace WebApi.Common.Mappings.Common;

/// <summary>
/// Configures the mapping between contract address and domain address.
/// </summary>
public class AddressMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ContractAddress, DomainAddress>()
            .MapWith(src => DomainAddress.Create(
                src.PostalCode,
                src.Street,
                src.State,
                src.City,
                src.Neighborhood)
            );
    }
}
