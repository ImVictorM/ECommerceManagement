using Application.Common.Interfaces.Payments;

namespace Infrastructure.Payments.Common.DTOs;

/// <summary>
/// Represents a response for an authorize payment request.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="Status">The status of the authorization.</param>
/// <param name="Details">The details of the authorization.</param>
/// <param name="Captured">A value indicating if the payment was captured.</param>
public record AuthorizePaymentResponse(
    string PaymentId,
    string Status,
    string Details,
    bool Captured
) : IAuthorizePaymentResponse;

