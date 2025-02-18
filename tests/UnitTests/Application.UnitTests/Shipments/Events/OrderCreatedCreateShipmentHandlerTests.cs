using Application.Common.Persistence;
using Application.Shipments.Events;
using Application.UnitTests.TestUtils.Events.Orders;

using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Moq;

namespace Application.UnitTests.Shipments.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCreatedCreateShipmentHandler"/> event handler.
/// </summary>
public class OrderCreatedCreateShipmentHandlerTests
{
    private readonly OrderCreatedCreateShipmentHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;
    private readonly Mock<IRepository<Carrier, CarrierId>> _mockCarrierRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedCreateShipmentHandlerTests"/> class.
    /// </summary>
    public OrderCreatedCreateShipmentHandlerTests()
    {
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();
        _mockCarrierRepository = new Mock<IRepository<Carrier, CarrierId>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.CarrierRepository).Returns(_mockCarrierRepository.Object);

        _handler = new OrderCreatedCreateShipmentHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Tests the handler creates a new shipment when the order is created.
    /// </summary>
    [Fact]
    public async Task HandleOrderPaid_WithPaidOrder_CreatesShipment()
    {
        var order = await OrderUtils.CreateOrderAsync(
            id: OrderId.Create(1),
            initialOrderStatus: OrderStatus.Paid
        );

        var shipmentCarrier = CarrierUtils.CreateCarrier(id: CarrierId.Create(1));

        _mockCarrierRepository.Setup(r => r.FirstAsync()).ReturnsAsync(shipmentCarrier);

        var notification = await OrderCreatedUtils.CreateEventAsync(order: order);

        await _handler.Handle(notification, default);

        _mockShipmentRepository.Verify(
            r => r.AddAsync(
                It.Is<Shipment>(s => s.OrderId == order.Id && s.CarrierId == shipmentCarrier.Id)
            ),
            Times.Once()
        );
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }
}
