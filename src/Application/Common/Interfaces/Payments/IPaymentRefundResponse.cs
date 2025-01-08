using Domain.PaymentAggregate.Enumerations;

namespace Application.Common.Interfaces.Payments;

/// <summary>
/// Represents a response containing refund data.
/// </summary>
public interface IPaymentRefundResponse
{
    /// <summary>
    /// The refund generated identifier.
    /// </summary>
    public string RefundId { get; }
    /// <summary>
    /// The payment identifier.
    /// </summary>
    public string PaymentId { get; }
    /// <summary>
    /// The amount refunded.
    /// </summary>
    public decimal Amount { get; }
    /// <summary>
    /// The status of the refund.
    /// </summary>
    public PaymentStatus Status { get; }
    /// <summary>
    /// The reason for the refund.
    /// </summary>
    public string Reason { get; }
    /// <summary>
    /// The refund mode.
    /// </summary>
    public string RefundMode { get; }
}
