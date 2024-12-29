using Application.Orders.Commands.Common.DTOs;
using Application.Orders.Commands.PlaceOrder;
using Application.Orders.Common.DTOs;

using Contracts.Orders;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configures mapping between order objects.
/// </summary>
public class OrderMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(string AuthenticatedUserId, PlaceOrderRequest Request), PlaceOrderCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.BillingAddress, src => src.Request.BillingAddress)
            .Map(dest => dest.CouponAppliedIds, src => src.Request.CouponAppliedIds)
            .Map(dest => dest.DeliveryAddress, src => src.Request.DeliveryAddress)
            .Map(dest => dest.Products, src => src.Request.Products.Select(p => new OrderProductInput(p.ProductId, p.Quantity)))
            .Map(dest => dest.CurrentUserId, src => src.AuthenticatedUserId)
            .Map(dest => dest.PaymentMethod, src => src.Request.PaymentMethod)
            .Map(dest => dest.Installments, src => src.Request.installments);

        config.NewConfig<OrderDetailedResult, OrderDetailedResponse>()
            .Map(dest => dest.Id, src => src.Order.Id.ToString())
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId.ToString())
            .Map(dest => dest.Total, src => src.Order.Total)
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.Status, src => src.Order.GetStatusDescription())
            .Map(dest => dest.Products, src => src.Order.Products)
            .Map(dest => dest.Payment, src => src.Payment == null ? null : new
            {
                PaymentId = src.Payment.Id.ToString(),
                Amount = src.Payment.Amount,
                Installments = src.Payment.Installments,
                Status = src.Payment.GetStatusDescription(),
                Description = src.Payment.Description,
                PaymentType = src.Payment.PaymentMethod.Type
            });
    }
}
