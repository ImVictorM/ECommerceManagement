using Application.Orders.Events;

using Domain.OrderAggregate.Enumerations;
using Domain.PaymentAggregate.Enumerations;
using Domain.UnitTests.TestUtils;
using Domain.OrderAggregate.ValueObjects;
using Domain.OrderAggregate;

using FluentAssertions;
using Moq;
using Application.UnitTests.TestUtils.Events.Payments;
using Application.Common.Persistence;

namespace Application.UnitTests.Orders.Events;

/// <summary>
/// Unit tests for the <see cref="PaymentCanceledCancelOrderHandler"/> handler.
/// </summary>
public class PaymentCanceledCancelOrderHandlerTests
{
    private readonly PaymentCanceledCancelOrderHandler _handler;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCanceledCancelOrderHandlerTests"/> class.
    /// </summary>
    public PaymentCanceledCancelOrderHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _handler = new PaymentCanceledCancelOrderHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the handler cancels the order and saves it when the order exists.
    /// </summary>
    [Fact]
    public async Task HandlePaymentCanceled_WhenOrderIsFound_CancelsIt()
    {
        var order = await OrderUtils.CreateOrderAsync(
            id: OrderId.Create(1),
            initialOrderStatus: OrderStatus.Pending
        );

        var payment = PaymentUtils.CreatePayment(orderId: order.Id, paymentStatus: PaymentStatus.Canceled);
        var notification = PaymentCanceledUtils.CreateEvent(payment);

        _mockOrderRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<OrderId>()))
            .ReturnsAsync(order);

        await _handler.Handle(notification, default);

        order.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);
        order.Description.Should().Be("The payment was canceled");
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
