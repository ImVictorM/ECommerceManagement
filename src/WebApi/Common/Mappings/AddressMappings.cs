using Contracts.Common;

using SharedKernel.ValueObjects;

using Mapster;

namespace WebApi.Common.Mappings;

internal sealed class AddressMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<AddressContract, Address>()
            .MapWith(src => Address.Create(
                src.PostalCode,
                src.Street,
                src.State,
                src.City,
                src.Neighborhood
            ));

        config.NewConfig<Address, AddressContract>();
    }
}
