namespace Application.Common.PaymentGateway.PaymentMethods;

/// <summary>
/// Represents a base card payment method.
/// </summary>
/// <param name="Token">The tokenized card data.</param>
/// <param name="Type">The derived payment method type.</param>
public abstract record CardPayment(
    string Token,
    string Type
) : PaymentMethod(Type);
