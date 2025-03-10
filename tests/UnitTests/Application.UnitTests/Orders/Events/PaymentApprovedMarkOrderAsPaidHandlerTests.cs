using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Orders.Events;
using Application.UnitTests.TestUtils.Events.Payments;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.Enumerations;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Events;

/// <summary>
/// Unit tests for the <see cref="PaymentApprovedMarkOrderAsPaidHandler"/> handler.
/// </summary>
public class PaymentApprovedMarkOrderAsPaidHandlerTests
{
    private readonly PaymentApprovedMarkOrderAsPaidHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentApprovedMarkOrderAsPaidHandlerTests"/> class.
    /// </summary>
    public PaymentApprovedMarkOrderAsPaidHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PaymentApprovedMarkOrderAsPaidHandler(
            _mockOrderRepository.Object,
            _mockUnitOfWork.Object
        );
    }

    /// <summary>
    /// Tests the handler marks the order as paid and saves it when the order exists.
    /// </summary>
    [Fact]
    public async Task HandlePaymentApproved_WhenOrderIsFound_MarksItAsPaid()
    {
        var order = await OrderUtils.CreateOrderAsync(
            id: OrderId.Create(1),
            initialOrderStatus: OrderStatus.Pending
        );

        var payment = PaymentUtils.CreatePayment(orderId: order.Id, paymentStatus: PaymentStatus.Approved);
        var notification = PaymentApprovedUtils.CreateEvent(payment);

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<OrderId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(order);

        await _handler.Handle(notification, default);

        order.OrderStatus.Should().Be(OrderStatus.Paid);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Tests the handler throws an exception when the order is not found.
    /// </summary>
    [Fact]
    public async Task HandlePaymentApproved_WhenOrderIsNotFound_ThrowsError()
    {
        var notification = PaymentApprovedUtils.CreateEvent();

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<OrderId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Order?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(notification, default))
            .Should()
            .ThrowAsync<OperationProcessFailedException>();
    }
}
