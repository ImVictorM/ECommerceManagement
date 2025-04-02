using Application.Common.PaymentGateway.Responses;
using Application.Common.PaymentGateway.PaymentMethods;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using Bogus;

namespace Application.UnitTests.TestUtils.PaymentGateway;

/// <summary>
/// Utilities for the <see cref="PaymentResponse"/> class.
/// </summary>
public static class PaymentResponseUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentResponse"/> class.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="amount">The amount paid.</param>
    /// <param name="installments">The installments.</param>
    /// <param name="paymentStatus">The current payment status.</param>
    /// <param name="details">The payment details.</param>
    /// <param name="captured">
    /// Boolean value indicating if the payment was captured.
    /// </param>
    /// <returns>
    /// A new instance of the <see cref="PaymentResponse"/> class.
    /// </returns>
    public static PaymentResponse CreateResponse(
        PaymentId? paymentId = null,
        PaymentMethod? paymentMethod = null,
        decimal? amount = null,
        int? installments = null,
        PaymentStatus? paymentStatus = null,
        string? details = null,
        bool captured = false
    )
    {
        return new PaymentResponse(
            paymentId ?? PaymentId.Create(_faker.Random.Guid().ToString()),
            paymentMethod ?? new CreditCard("tokenized-card-data"),
            amount ?? _faker.Random.Decimal(1m, 50m),
            installments ?? _faker.Random.Int(1, 12),
            paymentStatus ?? PaymentStatus.Pending,
            details ?? "Default payment details",
            captured
        );
    }
}
