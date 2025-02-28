using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using Contracts.Orders;

using Mapster;

namespace WebApi.Orders;

internal sealed class OrderMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid RequestId, PlaceOrderRequest Request), PlaceOrderCommand>()
            .MapWith(src => new PlaceOrderCommand(
                src.RequestId,
                src.Request.ShippingMethodId,
                src.Request.Products.Select(p => new OrderProductInput(
                    p.ProductId,
                    p.Quantity
                )),
                src.Request.BillingAddress.Adapt<Address>(),
                src.Request.DeliveryAddress.Adapt<Address>(),
                src.Request.PaymentMethod.Adapt<IPaymentMethod>(),
                src.Request.CouponAppliedIds,
                src.Request.installments
            ));

        config.NewConfig<OrderShippingMethodResult, OrderShippingMethodResponse>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.EstimatedDeliveryDays, src => src.EstimatedDeliveryDays);

        config.NewConfig<OrderShipmentResult, OrderShipmentResponse>()
            .Map(dest => dest.ShipmentId, src => src.ShipmentId.ToString())
            .Map(dest => dest.DeliveryAddress, src => src.DeliveryAddress)
            .Map(dest => dest.ShippingMethod, src => src.ShippingMethod)
            .Map(dest => dest.Status, src => src.Status);

        config.NewConfig<OrderPaymentResult, OrderPaymentResponse>()
            .Map(dest => dest.PaymentId, src => src.PaymentId.ToString())
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Installments, src => src.Installments)
            .Map(dest => dest.Status, src => src.Status)
            .Map(dest => dest.Description, src => src.Details)
            .Map(dest => dest.PaymentType, src => src.PaymentMethod);

        config.NewConfig<OrderDetailedResult, OrderDetailedResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Total, src => src.Order.Total)
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest.Products, src => src.Order.Products)
            .Map(dest => dest.Payment, src => src.Payment)
            .Map(dest => dest.Shipment, src => src.Shipment);

        config.NewConfig<OrderResult, OrderResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest.Total, src => src.Order.Total);
    }
}
