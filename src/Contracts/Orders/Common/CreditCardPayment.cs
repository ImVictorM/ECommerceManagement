namespace Contracts.Orders.Common;

/// <summary>
/// Represents a credit card payment method.
/// </summary>
/// <param name="Amount">The amount to be paid.</param>
public record CreditCardPayment(decimal Amount) : PaymentMethod("CreditCard", Amount);
