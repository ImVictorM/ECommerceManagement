namespace Contracts.Payments;

/// <summary>
/// Represents a payment status notification.
/// </summary>
/// <param name="PaymentId">The payment identifier.</param>
/// <param name="Status">The payment status.</param>
public record PaymentStatusChangedRequest(string PaymentId, string Status);
