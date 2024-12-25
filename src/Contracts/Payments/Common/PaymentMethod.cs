using System.Text.Json.Serialization;

namespace Contracts.Payments.Common;

/// <summary>
/// Represents a polymorphic payment method.
/// </summary>
/// <param name="Type">The type of the payment.</param>
[JsonPolymorphic]
[JsonDerivedType(typeof(CreditCardPayment), "CreditCard")]
[JsonDerivedType(typeof(DebitCardPayment), "DebitCard")]
[JsonDerivedType(typeof(PixPayment), "Pix")]
public record PaymentMethod(string Type);
