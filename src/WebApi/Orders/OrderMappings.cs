using Application.Common.PaymentGateway.PaymentMethods;
using Application.Orders.Commands.PlaceOrder;
using Application.Orders.DTOs.Inputs;
using Application.Orders.DTOs.Results;
using AppPMs = Application.Common.PaymentGateway.PaymentMethods;

using ContractPMs = Contracts.Common.PaymentMethods;
using Contracts.Common.PaymentMethods;
using Contracts.Orders;

using Mapster;

namespace WebApi.Orders;

internal sealed class OrderMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderLineItemRequest, OrderLineItemInput>();

        config
            .NewConfig<BasePaymentMethod, PaymentMethod>()
            .TwoWays()
            .MapToConstructor(true)
            .Include<ContractPMs.CreditCard, AppPMs.CreditCard>()
            .Include<ContractPMs.DebitCard, AppPMs.DebitCard>()
            .Include<ContractPMs.PaymentSlip, AppPMs.PaymentSlip>()
            .Include<ContractPMs.Pix, AppPMs.Pix>();

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
