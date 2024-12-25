namespace Contracts.Payments.Common;

/// <summary>
/// Represents a payment slip payment method.
/// </summary>
public record PaymentSlip() : PaymentMethod("PaymentSlip");
