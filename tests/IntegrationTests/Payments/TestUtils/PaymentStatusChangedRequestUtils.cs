using Domain.PaymentAggregate.Enumerations;

using Contracts.Payments;

using Bogus;

namespace IntegrationTests.Payments.TestUtils;

/// <summary>
/// Utilities for the <see cref="PaymentStatusChangedRequest"/> class.
/// </summary>
public static class PaymentStatusChangedRequestUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="PaymentStatusChangedRequest"/>
    /// class.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="paymentStatus">The payment status.</param>
    /// <returns>
    /// A new instance of the <see cref="PaymentStatusChangedRequest"/> class.
    /// </returns>
    public static PaymentStatusChangedRequest CreateRequest(
        string? paymentId = null,
        string? paymentStatus = null
    )
    {
        return new PaymentStatusChangedRequest(
            paymentId ?? _faker.Random.Guid().ToString(),
            paymentStatus ?? PaymentStatus.Authorized.Name
        );
    }
}
