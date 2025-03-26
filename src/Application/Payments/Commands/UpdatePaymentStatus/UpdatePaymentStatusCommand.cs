using MediatR;

namespace Application.Payments.Commands.UpdatePaymentStatus;

/// <summary>
/// Represents a command to update a payment status.
/// </summary>
/// <param name="PaymentId">The payment identifier.</param>
/// <param name="Status">The updated payment status.</param>
public record UpdatePaymentStatusCommand(
    string PaymentId,
    string Status
) : IRequest<Unit>;

