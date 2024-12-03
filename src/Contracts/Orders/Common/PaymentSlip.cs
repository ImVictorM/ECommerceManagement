namespace Contracts.Orders.Common;

/// <summary>
/// Represents a payment slip payment method.
/// </summary>
/// <param name="Amount">The amount to be paid.</param>
public record PaymentSlip(decimal Amount) : PaymentMethod("PaymentSlip", Amount);
