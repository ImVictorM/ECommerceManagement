using Application.Common.Interfaces.Persistence;
using Application.Shipments.Events;
using Application.UnitTests.TestUtils.Events.Orders;

using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Moq;

namespace Application.UnitTests.Shipments.Events;

/// <summary>
/// Unit tests for the <see cref="OrderPaidShipmentPreparationHandler"/> event handler.
/// </summary>
public class OrderPaidShipmentPreparationHandlerTests
{
    private readonly OrderPaidShipmentPreparationHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderPaidShipmentPreparationHandlerTests"/> class.
    /// </summary>
    public OrderPaidShipmentPreparationHandlerTests()
    {
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);

        _handler = new OrderPaidShipmentPreparationHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the handler creates a new shipment when the order is paid.
    /// </summary>
    [Fact]
    public async Task HandleOrderPaid_WithPaidOrder_CreatesShipment()
    {
        var order = await OrderUtils.CreateOrderAsync(
            id: OrderId.Create(1),
            initialOrderStatus: OrderStatus.Paid
        );

        var notification = await OrderPaidUtils.CreateEventAsync(order: order);

        await _handler.Handle(notification, default);

        _mockShipmentRepository.Verify(
            r => r.AddAsync(It.Is<Shipment>(s => s.OrderId == order.Id)),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
