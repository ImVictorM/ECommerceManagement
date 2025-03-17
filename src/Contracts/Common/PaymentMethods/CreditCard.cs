namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a credit card payment method.
/// </summary>
/// <param name="Token">The tokenized card data.</param>
public record CreditCard(string Token) : BaseCardPayment(Token, nameof(CreditCard));
