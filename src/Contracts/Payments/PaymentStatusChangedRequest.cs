namespace Contracts.Payments;

/// <summary>
/// Represents a payment status notification.
/// </summary>
/// <param name="PaymentId">The payment identifier.</param>
/// <param name="PaymentStatus">The payment status.</param>
public record PaymentStatusChangedRequest(string PaymentId, string PaymentStatus);
