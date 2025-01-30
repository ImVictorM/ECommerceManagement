namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a PIX payment method.
/// </summary>
public record PixPayment() : PaymentMethod("Pix");
