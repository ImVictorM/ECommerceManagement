using Domain.PaymentAggregate.Enumerations;

namespace Application.Common.PaymentGateway;

/// <summary>
/// Represents a generic response for payment requests.
/// </summary>
/// <param name="Status">The status of the request.</param>
/// <param name="Details">The details about the request.</param>
/// <param name="Captured">A value indicating if the payment was captured.</param>
public record PaymentStatusResponse
(
    PaymentStatus Status,
    string Details,
    bool Captured
);
