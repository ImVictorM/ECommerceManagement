namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a payment slip payment method.
/// </summary>
public record PaymentSlip() : BasePaymentMethod(nameof(PaymentSlip));
