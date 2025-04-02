using Domain.PaymentAggregate.Enumerations;

namespace Application.Common.PaymentGateway.Responses;

/// <summary>
/// Represents a payment status response.
/// </summary>
/// <param name="Status">The status of the payment.</param>
/// <param name="Details">The details about the payment.</param>
/// <param name="Captured">
/// A boolean value indicating if the payment was captured.
/// </param>
public record PaymentStatusResponse
(
    PaymentStatus Status,
    string Details,
    bool Captured
);
