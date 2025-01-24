using MediatR;

namespace Application.Payments.Commands.UpdatePaymentStatus;

/// <summary>
/// Command to update a payment status.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="Status">The payment status.</param>
public record UpdatePaymentStatusCommand(string PaymentId, string Status) : IRequest<Unit>;

