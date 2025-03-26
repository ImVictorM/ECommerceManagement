using Application.Common.PaymentGateway.PaymentMethods;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

namespace Application.Common.PaymentGateway.Responses;

/// <summary>
/// Represents a payment response.
/// </summary>
/// <param name="PaymentId">The payment identifier.</param>
/// <param name="PaymentMethod">The payment method.</param>
/// <param name="Amount">The payment amount.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="Status">The payment status.</param>
/// <param name="Details">The payment details.</param>
/// <param name="Captured">
/// A boolean value indicating if the payment was captured.
/// </param>
public record PaymentResponse
(
    PaymentId PaymentId,
    PaymentMethod PaymentMethod,
    decimal Amount,
    int Installments,
    PaymentStatus Status,
    string Details,
    bool Captured
) : PaymentStatusResponse(Status, Details, Captured);

