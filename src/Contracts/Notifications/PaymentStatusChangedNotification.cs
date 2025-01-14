namespace Contracts.Notifications;

/// <summary>
/// Represents a payment status notification.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="PaymentStatus">The payment status.</param>
public record PaymentStatusChangedNotification(string PaymentId, string PaymentStatus);
