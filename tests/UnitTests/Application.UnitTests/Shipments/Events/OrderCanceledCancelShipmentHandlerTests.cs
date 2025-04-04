using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Shipments.Events;

using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using FluentAssertions;
using Moq;

namespace Application.UnitTests.Shipments.Events;

/// <summary>
/// Unit tests for the <see cref="OrderCanceledCancelShipmentHandler"/>
/// event handler.
/// </summary>
public class OrderCanceledCancelShipmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IShipmentRepository> _mockShipmentRepository;
    private readonly OrderCanceledCancelShipmentHandler _handler;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="OrderCanceledCancelShipmentHandlerTests"/> class.
    /// </summary>
    public OrderCanceledCancelShipmentHandlerTests()
    {
        _mockShipmentRepository = new Mock<IShipmentRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new OrderCanceledCancelShipmentHandler(
            _mockUnitOfWork.Object,
            _mockShipmentRepository.Object
        );
    }

    /// <summary>
    /// Verifies the handler cancels the related shipment.
    /// </summary>
    [Fact]
    public async Task HandleOrderCanceled_WithExistentShipment_CancelsIt()
    {
        var orderId = OrderId.Create(1);
        var order = await OrderUtils.CreateOrderAsync(id: orderId);

        var shipment = ShipmentUtils.CreateShipment(
            id: ShipmentId.Create(1),
            orderId: orderId
        );

        _mockShipmentRepository
            .Setup(repo => repo.GetShipmentByOrderId(
                orderId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shipment);

        await _handler.Handle(new OrderCanceled(order), default);

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Canceled);
        _mockShipmentRepository.Verify(r => r.Update(shipment), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
