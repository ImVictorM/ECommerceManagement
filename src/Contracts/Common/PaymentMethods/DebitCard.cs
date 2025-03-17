namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a debit card payment method.
/// </summary>
/// <param name="Token">The tokenized card data.</param>
public record DebitCard(string Token) : BaseCardPayment(Token, nameof(DebitCard));
