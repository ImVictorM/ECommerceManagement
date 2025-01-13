using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Payment"/> class.
/// </summary>
public static class PaymentUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Payment"/> class.
    /// </summary>
    /// <param name="paymentId">The payment id.</param>
    /// <param name="orderId">The order id.</param>
    /// <param name="paymentStatus">The payment status.</param>
    /// <returns>A new instance of the <see cref="Payment"/> class.</returns>
    public static Payment CreatePayment(
        PaymentId? paymentId = null,
        OrderId? orderId = null,
        PaymentStatus? paymentStatus = null
    )
    {
        return Payment.Create(
            paymentId ?? PaymentId.Create(_faker.Random.Guid().ToString()),
            orderId ?? OrderId.Create(_faker.Random.Long()),
            paymentStatus ?? PaymentStatus.Pending
        );
    }
}
