namespace Contracts.Orders.Common;

/// <summary>
/// Represents a debit card payment method.
/// </summary>
/// <param name="Amount">The amount to be paid.</param>
public record DebitCardPayment(decimal Amount) : PaymentMethod("DebitCard", Amount);
