using Application.Common.Persistence;
using Application.Shipments.Events;
using Application.UnitTests.TestUtils.Events.Orders;

using Domain.CarrierAggregate.ValueObjects;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
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
    private readonly Mock<IShipmentRepository> _mockShipmentRepository;
    private readonly Mock<ICarrierRepository> _mockCarrierRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedCreateShipmentHandlerTests"/> class.
    /// </summary>
    public OrderCreatedCreateShipmentHandlerTests()
    {
        _mockShipmentRepository = new Mock<IShipmentRepository>();
        _mockCarrierRepository = new Mock<ICarrierRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new OrderCreatedCreateShipmentHandler(
            _mockUnitOfWork.Object,
            _mockCarrierRepository.Object,
            _mockShipmentRepository.Object
        );
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

        _mockCarrierRepository
            .Setup(r => r.FirstAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(shipmentCarrier);

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
