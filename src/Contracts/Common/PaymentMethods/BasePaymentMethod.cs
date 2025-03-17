using System.Text.Json.Serialization;

namespace Contracts.Common.PaymentMethods;

/// <summary>
/// Represents a polymorphic payment method.
/// </summary>
/// <param name="Type">The type of the payment.</param>
[JsonPolymorphic]
[JsonDerivedType(typeof(CreditCard), nameof(CreditCard))]
[JsonDerivedType(typeof(DebitCard), nameof(DebitCard))]
[JsonDerivedType(typeof(PaymentSlip), nameof(PaymentSlip))]
[JsonDerivedType(typeof(Pix), nameof(Pix))]
public abstract record BasePaymentMethod(string Type);
