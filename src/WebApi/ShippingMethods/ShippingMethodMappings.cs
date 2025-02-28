using Application.ShippingMethods.Commands.CreateShippingMethod;
using Application.ShippingMethods.Commands.UpdateShippingMethod;
using Application.ShippingMethods.DTOs;

using Contracts.ShippingMethods;

using Mapster;

namespace WebApi.ShippingMethods;

internal sealed class ShippingMethodMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateShippingMethodRequest, CreateShippingMethodCommand>();

        config.NewConfig<
                (string ShippingMethodId, UpdateShippingMethodRequest Request),
                UpdateShippingMethodCommand
            >()
            .Map(dest => dest.ShippingMethodId, src => src.ShippingMethodId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<ShippingMethodResult, ShippingMethodResponse>()
            .Map(dest => dest.Id, src => src.ShippingMethod.Id.ToString())
            .Map(dest => dest, src => src.ShippingMethod);
    }
}
