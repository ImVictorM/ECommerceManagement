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
            .Map(dest => dest.AuthenticatedUserId, src => src.AuthenticatedUserId)
            .Map(dest => dest.PaymentMethod, src => src.Request.PaymentMethod)
            .Map(dest => dest.Installments, src => src.Request.installments);

        config.NewConfig<OrderResult, OrderResponse>()
            .Map(dest => dest.Id, src => src.Order.Id)
            .Map(dest => dest.OwnerId, src => src.Order.OwnerId)
            .Map(dest => dest.BaseTotal, src => src.Order.Total)
            .Map(dest => dest.TotalWithDiscounts, src => src.Order.CalculateTotalApplyingDiscounts())
            .Map(dest => dest.Description, src => src.Order.Description)
            .Map(dest => dest.DiscountsApplied, src => src.Order.Discounts)
            .Map(dest => dest.Status, src => src.Order.GetStatusDescription());
    }
}
