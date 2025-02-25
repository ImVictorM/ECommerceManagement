using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Payments.Commands.UpdatePaymentStatus;
using Application.Payments.Errors;
using Application.UnitTests.Payments.Commands.TestUtils;

using Domain.PaymentAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Payments.Commands.UpdatePaymentStatus;

/// <summary>
/// Unit tests for the <see cref="UpdatePaymentStatusCommandHandler"/> handler.
/// </summary>
public class UpdatePaymentStatusCommandHandlerTests
{
    private readonly Mock<IPaymentRepository> _mockPaymentRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly UpdatePaymentStatusCommandHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="UpdatePaymentStatusCommandHandlerTests"/> class.
    /// </summary>
    public UpdatePaymentStatusCommandHandlerTests()
    {
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new UpdatePaymentStatusCommandHandler(
            _mockUnitOfWork.Object,
            _mockPaymentRepository.Object,
            new Mock<ILogger<UpdatePaymentStatusCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies when the payment does not exist a not found exception is thrown.
    /// </summary>
    [Fact]
    public async Task HandleUpdatePaymentStatus_WhenPaymentDoesNotExist_ThrowsError()
    {
        var command = UpdatePaymentStatusCommandUtils.CreateCommand();

        _mockPaymentRepository
            .Setup(r => r.FindByIdAsync(
                PaymentId.Create(command.PaymentId),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Payment?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<PaymentNotFoundException>();
    }

    /// <summary>
    /// Verifies the payment is updated correctly when it exists.
    /// </summary>
    [Fact]
    public async Task HandleUpdatePaymentStatus_WhenPaymentExists_UpdatesIt()
    {
        var newStatus = PaymentStatus.Authorized;
        var command = UpdatePaymentStatusCommandUtils.CreateCommand(status: newStatus.Name);
        var paymentId = PaymentId.Create(command.PaymentId);
        var payment = PaymentUtils.CreatePayment(paymentId: paymentId, paymentStatus: PaymentStatus.Pending);

        _mockPaymentRepository
            .Setup(r => r.FindByIdAsync(
                paymentId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(payment);

        await _handler.Handle(command, default);

        payment.PaymentStatus.Should().Be(newStatus);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
