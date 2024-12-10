using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumeration;
using Domain.UnitTests.TestUtils;
using Domain.UnitTests.TestUtils.Constants;
using Domain.UserAggregate.ValueObjects;
using FluentAssertions;
using SharedKernel.Errors;
using SharedKernel.Interfaces;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace Domain.UnitTests.PaymentAggregate;

/// <summary>
/// Tests for the <see cref="Payment"/> aggregate.
/// </summary>
public class PaymentTests
{
    /// <summary>
    /// Valid parameters to create payment instances.
    /// </summary>
    public static IEnumerable<object[]> ValidPaymentCreationParameters()
    {
        yield return new object[]
        {
            DomainConstants.Payment.Amount,
            DomainConstants.Payment.OrderId,
            DomainConstants.Payment.PayerId,
            PaymentUtils.CreateCreditCardPayment(),
            AddressUtils.CreateAddress(),
            AddressUtils.CreateAddress(),
            DomainConstants.Payment.Installments
        };
    }

    /// <summary>
    /// Status a payment should have to be able to be canceled.
    /// </summary>
    public static IEnumerable<object[]> ValidPaymentStatusToCancelPayment()
    {
        yield return new object[] { PaymentStatus.InProgress };
        yield return new object[] { PaymentStatus.Pending };
    }

    /// <summary>
    /// Status a payment should not have to be able to be canceled.
    /// </summary>
    public static IEnumerable<object[]> InvalidPaymentStatusToCancelPayment()
    {
        yield return new object[] { PaymentStatus.Refunded };
        yield return new object[] { PaymentStatus.Canceled };
        yield return new object[] { PaymentStatus.Rejected };
        yield return new object[] { PaymentStatus.Approved };
    }

    /// <summary>
    /// Tests that the payment instance is created correctly.
    /// </summary>
    /// <param name="amount">The payment amount.</param>
    /// <param name="orderId">Tha payment order id.</param>
    /// <param name="payerId">Tha payment payer id.</param>
    /// <param name="paymentMethod">The payment method.</param>
    /// <param name="billingAddress">The payment billing address.</param>
    /// <param name="deliveryAddress">The delivery address.</param>
    /// <param name="installments">The quantity of installments.</param>
    [Theory]
    [MemberData(nameof(ValidPaymentCreationParameters))]
    public void Payment_WhenCreatingNew_ReturnsInstanceWithCorrectConfiguration(
        decimal amount,
        OrderId orderId,
        UserId payerId,
        IPaymentMethod paymentMethod,
        Address billingAddress,
        Address deliveryAddress,
        int installments
    )
    {
        var act = FluentActions
            .Invoking(() => Payment.Create(
                amount,
                orderId,
                payerId,
                paymentMethod,
                billingAddress,
                deliveryAddress,
                installments
            ))
            .Should()
            .NotThrow();

        var payment = act.Subject;

        payment.Amount.Should().Be(amount);
        payment.Installments.Should().Be(installments);
        payment.OrderId.Should().Be(orderId);
        payment.PaymentMethod.Should().Be(paymentMethod);
        payment.PaymentStatusId.Should().Be(PaymentStatus.InProgress.Id);
        payment.Description.Should().Be("Payment in progress. Waiting for authorization");
        payment.PaymentStatusHistories.Count.Should().Be(1);
        payment.PaymentStatusHistories.Should().Contain(psh => psh.PaymentStatusId == PaymentStatus.InProgress.Id);
    }

    /// <summary>
    /// Tests when creating a payment with null installments it defaults to one.
    /// </summary>
    [Fact]
    public void Payment_WhenCreatingWithNullInstallments_DefaultsToOne()
    {
        var payment = PaymentUtils.CreatePayment(installments: null);

        payment.Installments.Should().Be(1);
    }

    /// <summary>
    /// Tests that is possible to cancel a payment only if the current payment status is pending or in-progress.
    /// </summary>
    /// <param name="validPaymentStatus">The payment initial status.</param>
    [Theory]
    [MemberData(nameof(ValidPaymentStatusToCancelPayment))]
    public void Payment_WhenCancelingPendingOrInProgressPayment_UpdatesPaymentCorrectly(PaymentStatus validPaymentStatus)
    {
        var payment = PaymentUtils.CreatePayment(initialPaymentStatus: validPaymentStatus);

        payment.CancelPayment("Important reason");

        payment.PaymentStatusId.Should().Be(PaymentStatus.Canceled.Id);
        payment.PaymentStatusHistories.Count.Should().Be(2);
        payment.PaymentStatusHistories.Should().Contain(psh => psh.PaymentStatusId == PaymentStatus.Canceled.Id);
    }

    /// <summary>
    /// Tests that it is not possible to cancel a payment if the current status is different than pending or in-progress.
    /// </summary>
    /// <param name="invalidPaymentStatus">The payment initial status.</param>
    [Theory]
    [MemberData(nameof(InvalidPaymentStatusToCancelPayment))]
    public void Payment_WhenCancelingPaymentThatIsNotPendingOrInProgress_ThrowsError(PaymentStatus invalidPaymentStatus)
    {
        var payment = PaymentUtils.CreatePayment(initialPaymentStatus: invalidPaymentStatus);

        FluentActions
            .Invoking(() => payment.CancelPayment("Important reason"))
            .Should()
            .Throw<BaseException>()
            .WithMessage("The current payment cannot be canceled. Only payments pending or in progress can be canceled");
    }
}
