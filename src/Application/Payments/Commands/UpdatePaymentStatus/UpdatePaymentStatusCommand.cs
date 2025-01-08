using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using MediatR;

namespace Application.Payments.Commands.UpdatePaymentStatus;

/// <summary>
/// Command to update a payment status.
/// </summary>
public class UpdatePaymentStatusCommand : IRequest<Unit>
{
    /// <summary>
    /// Gets the payment id.
    /// </summary>
    public PaymentId PaymentId { get; }
    /// <summary>
    /// Gets the payment status.
    /// </summary>
    public PaymentStatus Status { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdatePaymentStatusCommand"/> class.
    /// </summary>
    /// <param name="paymentId">The payment id.</param>
    /// <param name="status">The payment status.</param>
    public UpdatePaymentStatusCommand(string paymentId, string status)
    {
        PaymentId = PaymentId.Create(paymentId);
        Status = PaymentStatus.FromDisplayName(status);
    }
}
