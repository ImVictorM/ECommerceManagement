using Application.Common.Errors;
using Application.Common.Persistence;
using Application.Shipments.Events;

using Domain.OrderAggregate.Events;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using System.Linq.Expressions;
using FluentAssertions;
using Moq;

namespace Application.UnitTests.Shipments.Events;

/// <summary>
/// Unit tests for the <see cref="OrderPaidPrepareShipmentHandler"/> event handler.
/// </summary>
public class OrderPaidPrepareShipmentHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;
    private readonly OrderPaidPrepareShipmentHandler _handler;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderPaidPrepareShipmentHandlerTests"/> class.
    /// </summary>
    public OrderPaidPrepareShipmentHandlerTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();

        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);

        _handler = new OrderPaidPrepareShipmentHandler(_mockUnitOfWork.Object);
    }

    /// <summary>
    /// Verifies when the shipment is pending it is updated to preparing.
    /// </summary>
    [Fact]
    public async Task HandleOrderPaid_WithPendingShipment_AdvancesToPreparing()
    {
        var orderId = OrderId.Create(1);
        var order = await OrderUtils.CreateOrderAsync(id: orderId);
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), orderId: orderId);

        _mockShipmentRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Shipment, bool>>>()))
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
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), orderId: orderId);

        shipment.AdvanceShipmentStatus();

        _mockShipmentRepository
            .Setup(repo => repo.FindOneOrDefaultAsync(It.IsAny<Expression<Func<Shipment, bool>>>()))
            .ReturnsAsync(shipment);

        await FluentActions
            .Invoking(() => _handler.Handle(new OrderPaid(order), default))
            .Should()
            .ThrowAsync<OperationProcessFailedException>()
            .WithMessage($"Shipment status was expected to be 'Pending' but was 'Preparing' instead");

        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}
