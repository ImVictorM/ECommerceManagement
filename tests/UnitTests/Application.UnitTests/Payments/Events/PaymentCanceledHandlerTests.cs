using Application.Common.Interfaces.Persistence;
using Application.Payments.Events;
using Application.UnitTests.Payments.Events.TestUtils;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Payments.Events;

/// <summary>
/// Tests for the <see cref="PaymentCanceledHandler"/> event handler.
/// </summary>
public class PaymentCanceledHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly PaymentCanceledHandler _eventHandler;
    private readonly Mock<IRepository<Order, OrderId>> _mockOrderRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentCanceledHandlerTests"/> class.
    /// </summary>
    public PaymentCanceledHandlerTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order, OrderId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.OrderRepository).Returns(_mockOrderRepository.Object);

        _eventHandler = new PaymentCanceledHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests when the payment event is triggered the corresponding order is also canceled, updating the order status
    /// and description, verifying the changes are persisted.
    /// </summary>
    [Fact]
    public async Task HandlePaymentCanceled_WhenEventIsFired_CancelsTheRelatedOrder()
    {
        var orderToCancel = await OrderUtils.CreateOrder();
        var paymentCanceledEvent = PaymentCanceledUtils.CreateEvent();

        _mockOrderRepository.Setup(r => r.FindByIdAsync(It.IsAny<OrderId>())).ReturnsAsync(orderToCancel);

        await _eventHandler.Handle(paymentCanceledEvent, default);

        orderToCancel.OrderStatusId.Should().Be(OrderStatus.Canceled.Id);
        orderToCancel.Description.Should().Be("Payment canceled");

        _mockOrderRepository.Verify(r => r.UpdateAsync(orderToCancel), Times.Once());
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
