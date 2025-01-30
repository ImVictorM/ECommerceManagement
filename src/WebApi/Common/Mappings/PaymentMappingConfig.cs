using Application.Payments.Commands.UpdatePaymentStatus;
using Contracts.Payments;
using Mapster;

namespace WebApi.Common.Mappings;

/// <summary>
/// Configures the mappings between payment objects.
/// </summary>
public class PaymentMappingConfig : IRegister
{
    /// <inheritdoc/>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentStatusChangedRequest, UpdatePaymentStatusCommand>()
            .MapWith(src => new UpdatePaymentStatusCommand(src.PaymentId, src.PaymentStatus));
    }
}
