using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.ShippingMethods.Commands.UpdateShippingMethod;
using Application.ShippingMethods.DTOs.Results;

using Contracts.ShippingMethods;

using Mapster;

namespace WebApi.ShippingMethods;

internal sealed class ShippingMethodMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateShippingMethodRequest, CreateShippingMethodCommand>();

        config.NewConfig<
                (string Id, UpdateShippingMethodRequest Request),
                UpdateShippingMethodCommand
            >()
            .Map(dest => dest.ShippingMethodId, src => src.Id)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<ShippingMethodResult, ShippingMethodResponse>();
    }
}
