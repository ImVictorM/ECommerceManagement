using Application.Common.Persistence;
using Application.Shipments.Commands.AdvanceShipmentStatus;
using Application.Shipments.Errors;
using Application.UnitTests.Shipments.Commands.TestUtils;

using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Microsoft.Extensions.Logging;
using FluentAssertions;

using Moq;

namespace Application.UnitTests.Shipments.Commands.AdvanceShipmentStatus;

/// <summary>
/// Unit tests for the <see cref="AdvanceShipmentStatusCommandHandler"/> command handler.
/// </summary>
public class AdvanceShipmentStatusCommandHandlerTests
{
    private readonly Mock<IRepository<Shipment, ShipmentId>> _mockShipmentRepository;
    private readonly AdvanceShipmentStatusCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="AdvanceShipmentStatusCommandHandlerTests"/> class.
    /// </summary>
    public AdvanceShipmentStatusCommandHandlerTests()
    {
        _mockShipmentRepository = new Mock<IRepository<Shipment, ShipmentId>>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(uow => uow.ShipmentRepository).Returns(_mockShipmentRepository.Object);

        _handler = new AdvanceShipmentStatusCommandHandler(
            _mockUnitOfWork.Object,
            new Mock<ILogger<AdvanceShipmentStatusCommandHandler>>().Object
        );
    }

    /// <summary>
    /// Verifies the shipment status advances when the shipment exists and is not pending.
    /// </summary>
    [Fact]
    public async Task HandleAdvanceShipmentStatus_WithExistingPreparingShipment_AdvancesStatusToTheNextState()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1), initialShipmentStatus: ShipmentStatus.Preparing);

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShipmentId>()))
            .ReturnsAsync(shipment);

        await _handler.Handle(command, default);

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Shipped);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once());
    }

    /// <summary>
    /// Verifies an error is thrown when trying to advance a pending shipment manually.
    /// </summary>
    [Fact]
    public async Task HandleAdvanceShipmentStatus_WithExistingPendingShipment_ThrowsError()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1));

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShipmentId>()))
            .ReturnsAsync(shipment);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<AdvancePendingShipmentStatusException>();
    }

    /// <summary>
    /// Verifies an error is thrown when the shipment does not exist.
    /// </summary>
    [Fact]
    public async Task HandleAdvanceShipmentStatus_WhenShipmentDoesNotExist_ThrowsError()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(It.IsAny<ShipmentId>()))
            .ReturnsAsync((Shipment?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ShipmentNotFoundException>();
    }
}
