namespace Application.Common.PaymentGateway.PaymentMethods;

/// <summary>
/// Represents a PIX payment method.
/// </summary>
public record Pix() : PaymentMethod(nameof(Pix));
