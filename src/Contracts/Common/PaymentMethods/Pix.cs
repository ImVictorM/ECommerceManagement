namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a PIX payment method.
/// </summary>
public record Pix() : BasePaymentMethod(nameof(Pix));
