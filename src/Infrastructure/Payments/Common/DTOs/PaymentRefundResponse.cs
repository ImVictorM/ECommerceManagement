using Application.Common.Interfaces.Payments;

namespace Infrastructure.Payments.Common.DTOs;

/// <summary>
/// Represents a response for a refund payment request.
/// </summary>
/// <param name="RefundId">The refund generated id.</param>
/// <param name="PaymentId">The payment refunded id.</param>
/// <param name="Amount">The amount refunded.</param>
/// <param name="Status">The refund status.</param>
/// <param name="Reason">The reason for the refund.</param>
/// <param name="RefundMode">The refund mode.</param>
public record PaymentRefundResponse(
    string RefundId,
    string PaymentId,
    decimal Amount,
    string Status,
    string Reason,
    string RefundMode
) : IPaymentRefundResponse;

