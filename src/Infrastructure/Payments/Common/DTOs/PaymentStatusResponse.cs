using Application.Common.Interfaces.Payments;
using Domain.PaymentAggregate.Enumerations;

namespace Infrastructure.Payments.Common.DTOs;

/// <summary>
/// Represents a generic response for payment requests.
/// </summary>
/// <param name="Status">The status of the request.</param>
/// <param name="Details">The details about the request.</param>
/// <param name="Captured">A value indicating if the payment was captured.</param>
public record PaymentStatusResponse(
    PaymentStatus Status,
    string Details,
    bool Captured
) : IPaymentStatusResponse;

