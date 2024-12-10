using Application.Orders.Commands.PlaceOrder;
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
    }
}
