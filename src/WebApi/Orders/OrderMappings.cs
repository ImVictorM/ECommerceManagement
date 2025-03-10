using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs;

using Contracts.Orders;

using Mapster;

namespace WebApi.Orders;

internal sealed class OrderMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<(Guid RequestId, PlaceOrderRequest Request), PlaceOrderCommand>()
            .Map(dest => dest.RequestId, src => src.RequestId)
            .Map(dest => dest, src => src.Request);

        config.NewConfig<OrderShippingMethodResult, OrderShippingMethodResponse>();

        config
            .NewConfig<OrderShipmentResult, OrderShipmentResponse>()
            .Map(dest => dest.ShipmentId, src => src.ShipmentId.ToString());

        config.NewConfig<OrderPaymentResult, OrderPaymentResponse>()
            .Map(dest => dest.PaymentId, src => src.PaymentId.ToString())
            .Map(dest => dest.Description, src => src.Details)
            .Map(dest => dest.PaymentType, src => src.PaymentMethod);

        config.NewConfig<OrderDetailedResult, OrderDetailedResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest, src => src.Order);

        config.NewConfig<OrderResult, OrderResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest, src => src.Order);
    }
}
