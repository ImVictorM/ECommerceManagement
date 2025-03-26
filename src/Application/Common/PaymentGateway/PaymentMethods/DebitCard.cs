namespace Application.Common.PaymentGateway.PaymentMethods;

/// <summary>
/// Represents a debit card payment method.
/// </summary>
/// <param name="Token">The tokenized card data.</param>
public record DebitCard(string Token) : CardPayment(Token, nameof(DebitCard));
