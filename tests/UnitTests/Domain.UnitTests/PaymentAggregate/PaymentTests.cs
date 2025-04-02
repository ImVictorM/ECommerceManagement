using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.Events;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;

namespace Domain.UnitTests.PaymentAggregate;

/// <summary>
/// Unit tests for the <see cref="Domain.PaymentAggregate.Payment"/> class.
/// </summary>
public class PaymentTests
{
    /// <summary>
    /// Provides a list of valid payment creation parameters.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidPaymentCreationParameters =
    [
        [
            PaymentId.Create(Guid.NewGuid().ToString()),
            OrderId.Create(11),
            PaymentStatus.InProgress
        ],
        [
            PaymentId.Create("1"),
            OrderId.Create(5),
            PaymentStatus.Pending
        ],
        [
            PaymentId.Create("XYZ"),
            OrderId.Create(10),
            PaymentStatus.Authorized
        ]
    ];

    /// <summary>
    /// Provides a list of payment status with expected generated domain event type.
    /// </summary>
    public static readonly IEnumerable<object[]> PaymentStatusTransitionData =
    [
        [
            PaymentStatus.Authorized,
            typeof(PaymentAuthorized)
        ],
        [
            PaymentStatus.Approved,
            typeof(PaymentApproved)
        ],
        [
            PaymentStatus.Rejected,
            typeof(PaymentRejected)
        ],
        [
            PaymentStatus.Canceled,
            typeof(PaymentCanceled)
        ]
    ];

    /// <summary>
    /// Verifies it is possible to create a payment with valid parameters.
    /// </summary>
    /// <param name="paymentId">The payment identifier.</param>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="paymentStatus">The payment status.</param>
    [Theory]
    [MemberData(nameof(ValidPaymentCreationParameters))]
    public void Create_WithValidParameters_CreatesWithoutThrowing(
        PaymentId paymentId,
        OrderId orderId,
        PaymentStatus paymentStatus
    )
    {
        var actionResult = FluentActions
            .Invoking(() => PaymentUtils.CreatePayment(
                paymentId,
                orderId,
                paymentStatus
            ))
            .Should()
            .NotThrow();

        var payment = actionResult.Subject;

        payment.Should().NotBeNull();
        payment.Id.Should().Be(paymentId);
        payment.OrderId.Should().Be(orderId);
        payment.PaymentStatus.Should().Be(paymentStatus);
    }

    /// <summary>
    /// Verifies creating the payment with different status raises the correct
    /// domain event.
    /// </summary>
    /// <param name="status">The initial payment status.</param>
    /// <param name="expectedRaisedEventType">
    /// The expected event raised.
    /// </param>
    [Theory]
    [MemberData(nameof(PaymentStatusTransitionData))]
    public void Create_WithDifferentInitialStatus_RaisesTheCorrectDomainEvent(
        PaymentStatus status,
        Type expectedRaisedEventType
    )
    {
        var payment = PaymentUtils.CreatePayment(paymentStatus: status);

        payment.PaymentStatus.Should().Be(status);
        payment.DomainEvents
            .Should().Contain(e => e.GetType() == expectedRaisedEventType);
    }

    /// <summary>
    /// Verifies updating the payment status also generates a domain event.
    /// </summary>
    [Theory]
    [MemberData(nameof(PaymentStatusTransitionData))]
    public void UpdatePaymentStatus_WithDifferentStatuses_ShouldUpdateAndRaisesCorrectEvent(
        PaymentStatus status,
        Type expectedRaisedEventType
    )
    {
        var payment = PaymentUtils.CreatePayment(
            paymentStatus: PaymentStatus.Pending
        );

        payment.UpdatePaymentStatus(status);

        payment.PaymentStatus.Should().Be(status);
        payment.DomainEvents
            .Should().Contain(e => e.GetType() == expectedRaisedEventType);
    }
}
