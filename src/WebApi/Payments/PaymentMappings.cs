using Application.Payments.Commands.UpdatePaymentStatus;

using Contracts.Payments;

using Mapster;

namespace WebApi.Payments;

internal sealed class PaymentMappings : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentStatusChangedRequest, UpdatePaymentStatusCommand>()
            .MapWith(src => new UpdatePaymentStatusCommand(
                src.PaymentId,
                src.PaymentStatus
            ));
    }
}
