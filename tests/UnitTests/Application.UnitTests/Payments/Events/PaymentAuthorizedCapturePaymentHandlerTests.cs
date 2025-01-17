using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Application.Payments.Events;
using Application.UnitTests.TestUtils.Events.Payments;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Payments.Events;

/// <summary>
/// Unit tests for the <see cref="PaymentAuthorizedCapturePaymentHandler"/> handler.
/// </summary>
public class PaymentAuthorizedCapturePaymentHandlerTests
{
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly PaymentAuthorizedCapturePaymentHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentAuthorizedCapturePaymentHandlerTests"/> class.
    /// </summary>
    public PaymentAuthorizedCapturePaymentHandlerTests()
    {
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PaymentAuthorizedCapturePaymentHandler(_mockUnitOfWork.Object, _mockPaymentGateway.Object);
    }

    /// <summary>
    /// Verifies the payment is captured and updated with the response status.
    /// </summary>
    [Fact]
    public async Task HandlePaymentAuthorized_WhenEventIsFired_CapturesThePaymentAndUpdatesTheStatus()
    {
        var payment = PaymentUtils.CreatePayment(
            paymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            paymentStatus: PaymentStatus.Authorized
        );

        var mockPaymentStatusResponse = new Mock<PaymentStatusResponse>();

        mockPaymentStatusResponse.SetupGet(x => x.Status).Returns(PaymentStatus.Approved);

        _mockPaymentGateway
            .Setup(g => g.CapturePaymentAsync(payment.Id))
            .ReturnsAsync(mockPaymentStatusResponse.Object);

        var notification = PaymentAuthorizedUtils.CreateEvent(payment: payment);

        await _handler.Handle(notification, default);

        _mockPaymentGateway.Verify(g => g.CapturePaymentAsync(payment.Id), Times.Once());
        payment.PaymentStatusId.Should().Be(mockPaymentStatusResponse.Object.Status.Id);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
