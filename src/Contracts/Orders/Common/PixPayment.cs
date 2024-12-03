namespace Contracts.Orders.Common;

/// <summary>
/// Represents a PIX payment method.
/// </summary>
/// <param name="Amount">The amount to be paid.</param>
public record PixPayment(decimal Amount) : PaymentMethod("Pix", Amount);
