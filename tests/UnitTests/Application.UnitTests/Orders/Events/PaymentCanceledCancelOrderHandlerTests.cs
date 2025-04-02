using Application.Orders.Events;
using Application.UnitTests.TestUtils.Events.Payments;
using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;

using Domain.OrderAggregate.Enumerations;
using Domain.PaymentAggregate.Enumerations;
using Domain.UnitTests.TestUtils;
using Domain.OrderAggregate.ValueObjects;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Orders.Events;

/// <summary>
/// Unit tests for the <see cref="PaymentCanceledCancelOrderHandler"/> handler.
/// </summary>
public class PaymentCanceledCancelOrderHandlerTests
{
    private readonly PaymentCanceledCancelOrderHandler _handler;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="PaymentCanceledCancelOrderHandlerTests"/> class.
    /// </summary>
    public PaymentCanceledCancelOrderHandlerTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new PaymentCanceledCancelOrderHandler(
            _mockUnitOfWork.Object,
            _mockOrderRepository.Object
        );
    }

    /// <summary>
    /// Verifies the handler cancels the order and saves it when the order exists.
    /// </summary>
    [Fact]
    public async Task HandlePaymentCanceled_WithExistentOrder_CancelsIt()
    {
        var order = await OrderUtils.CreateOrderAsync(
            id: OrderId.Create(1),
            initialOrderStatus: OrderStatus.Pending
        );

        var payment = PaymentUtils.CreatePayment(
            orderId: order.Id,
            paymentStatus: PaymentStatus.Canceled
        );
        var notification = PaymentCanceledUtils.CreateEvent(payment);

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<OrderId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(order);

        await _handler.Handle(notification, default);

        order.OrderStatus.Should().Be(OrderStatus.Canceled);
        order.Description.Should().Be("The payment was canceled");
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
