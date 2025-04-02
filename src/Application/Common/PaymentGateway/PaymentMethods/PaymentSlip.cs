namespace Application.Common.PaymentGateway.PaymentMethods;

/// <summary>
/// Represents a payment slip payment method.
/// </summary>
public record PaymentSlip() : PaymentMethod(nameof(PaymentSlip));

