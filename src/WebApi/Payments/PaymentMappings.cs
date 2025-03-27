using Application.Payments.Commands.UpdatePaymentStatus;

using Contracts.Payments;

using Mapster;

namespace WebApi.Payments;

internal sealed class PaymentMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentStatusChangedRequest, UpdatePaymentStatusCommand>();
    }
}
