using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Shipments.Errors;
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
/// Unit tests for the <see cref="OrderPaidPrepareShipmentHandler"/>
/// event handler.
/// </summary>
public class OrderPaidPrepareShipmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IShipmentRepository> _mockShipmentRepository;
    private readonly OrderPaidPrepareShipmentHandler _handler;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="OrderPaidPrepareShipmentHandlerTests"/> class.
    /// </summary>
    public OrderPaidPrepareShipmentHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockShipmentRepository = new Mock<IShipmentRepository>();

        _handler = new OrderPaidPrepareShipmentHandler(
            _mockUnitOfWork.Object,
            _mockShipmentRepository.Object
        );
    }

    /// <summary>
    /// Verifies when the shipment is pending it is updated to preparing.
    /// </summary>
    [Fact]
    public async Task HandleOrderPaid_WithPendingShipment_AdvancesToPreparing()
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

        await _handler.Handle(new OrderPaid(order), default);

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Preparing);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies when the shipment is not pending an error is thrown.
    /// </summary>
    [Fact]
    public async Task HandleOrderPaid_WhenShipmentIsNotPending_ThrowsError()
    {
        var orderId = OrderId.Create(1);
        var order = await OrderUtils.CreateOrderAsync(id: orderId);
        var shipment = ShipmentUtils.CreateShipment(
            id: ShipmentId.Create(1),
            orderId: orderId
        );

        shipment.AdvanceShipmentStatus();

        _mockShipmentRepository
            .Setup(repo => repo.GetShipmentByOrderId(
                orderId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shipment);

        await FluentActions
            .Invoking(() => _handler.Handle(new OrderPaid(order), default))
            .Should()
            .ThrowAsync<PrepareShipmentNotPendingException>();

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}
