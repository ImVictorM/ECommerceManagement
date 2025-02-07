using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

using Contracts.Orders;

using Mapster;

namespace WebApi.Orders;

/// <summary>
/// Configures the mappings between order objects.
/// </summary>
public class OrderMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid RequestId, PlaceOrderRequest Request), PlaceOrderCommand>()
            .MapWith(src => new PlaceOrderCommand(
                src.RequestId,
                src.Request.ShippingMethodId,
                src.Request.Products.Select(p => new OrderProductInput(p.ProductId, p.Quantity)),
                src.Request.BillingAddress.Adapt<Address>(),
                src.Request.DeliveryAddress.Adapt<Address>(),
                src.Request.PaymentMethod.Adapt<IPaymentMethod>(),
                src.Request.CouponAppliedIds,
                src.Request.installments
            ));

        config.NewConfig<OrderDetailedResult, OrderDetailedResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Total, src => src.Order.Total)
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest.Products, src => src.Order.Products)
            .Map(dest => dest.Payment, src => src.Payment == null ? null : new OrderPaymentResponse(
                src.Payment.PaymentId,
                src.Payment.Amount,
                src.Payment.Installments,
                src.Payment.Status.Name,
                src.Payment.Details,
                src.Payment.PaymentMethod
            ))
            .IgnoreNullValues(true);

        config.NewConfig<OrderResult, OrderResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.Status, src => src.Order.OrderStatus.Name)
            .Map(dest => dest.Total, src => src.Order.Total);
    }
}
