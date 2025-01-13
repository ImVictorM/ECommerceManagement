using Application.Common.Interfaces.Payments;
using Application.Common.Interfaces.Persistence;
using Application.Payments.Events;
using Application.UnitTests.TestUtils.Events.Payments;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Payments.Events;

/// <summary>
/// Unit tests for the <see cref="PaymentRejectedCancelAuthorizationHandler"/> event handler.
/// </summary>
public class PaymentRejectedCancelAuthorizationHandlerTests
{
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly PaymentRejectedCancelAuthorizationHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentRejectedCancelAuthorizationHandlerTests"/> class.
    /// </summary>
    public PaymentRejectedCancelAuthorizationHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PaymentRejectedCancelAuthorizationHandler(
            _mockUnitOfWork.Object,
            _mockPaymentGateway.Object
        );
    }

    /// <summary>
    /// Verifies the handler cancels the authorization and updates the payment status.
    /// </summary>
    [Fact]
    public async Task HandlePaymentReject_WithRejectedPayment_CancelsTheAuthorizationAndUpdatesThePaymentStatus()
    {
        var payment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            paymentStatus: PaymentStatus.Rejected
        );

        var notification = PaymentRejectedUtils.CreateEvent(payment);

        var mockPaymentStatusResponse = new Mock<IPaymentStatusResponse>();
        mockPaymentStatusResponse.SetupGet(x => x.Status).Returns(PaymentStatus.Canceled);

        _mockPaymentGateway.Setup(g => g.CancelAuthorizationAsync(payment.Id)).ReturnsAsync(mockPaymentStatusResponse.Object);

        await _handler.Handle(notification, default);

        payment.PaymentStatusId.Should().Be(mockPaymentStatusResponse.Object.Status.Id);
        _mockPaymentGateway.Verify(g => g.CancelAuthorizationAsync(payment.Id), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
