namespace Contracts.Payments.Common;

/// <summary>
/// Represents a PIX payment method.
/// </summary>
public record PixPayment() : PaymentMethod("Pix");
