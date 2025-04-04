using SharedKernel.Interfaces;

using System.Text.Json.Serialization;

namespace Application.Common.PaymentGateway.PaymentMethods;

/// <summary>
/// Represents a polymorphic payment method.
/// </summary>
/// <param name="Name">The payment method name.</param>
[JsonPolymorphic]
[JsonDerivedType(typeof(CreditCard), nameof(CreditCard))]
[JsonDerivedType(typeof(DebitCard), nameof(DebitCard))]
[JsonDerivedType(typeof(PaymentSlip), nameof(PaymentSlip))]
[JsonDerivedType(typeof(Pix), nameof(Pix))]
public abstract record PaymentMethod(string Name) : IPaymentMethod;
