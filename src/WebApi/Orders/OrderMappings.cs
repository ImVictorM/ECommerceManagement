using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs.Results;

using Contracts.Orders;

using Mapster;

namespace WebApi.Orders;

internal sealed class OrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(Guid RequestId, PlaceOrderRequest Request), PlaceOrderCommand>()
            .Map(dest => dest.RequestId, src => src.RequestId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<OrderShippingMethodResult, OrderShippingMethodResponse>();
        config.NewConfig<OrderShipmentResult, OrderShipmentResponse>();
        config.NewConfig<OrderPaymentResult, OrderPaymentResponse>();
        config.NewConfig<OrderDetailedResult, OrderDetailedResponse>();
        config.NewConfig<OrderResult, OrderResponse>();
    }
}
