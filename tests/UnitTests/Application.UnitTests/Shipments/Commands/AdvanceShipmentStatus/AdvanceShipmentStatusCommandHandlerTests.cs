using Application.Common.Persistence.Repositories;
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
/// Unit tests for the <see cref="AdvanceShipmentStatusCommandHandler"/>
/// command handler.
/// </summary>
public class AdvanceShipmentStatusCommandHandlerTests
{
    private readonly Mock<IShipmentRepository> _mockShipmentRepository;
    private readonly AdvanceShipmentStatusCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    /// <summary>
    /// Initiates a new instance of the
    /// <see cref="AdvanceShipmentStatusCommandHandlerTests"/> class.
    /// </summary>
    public AdvanceShipmentStatusCommandHandlerTests()
    {
        _mockShipmentRepository = new Mock<IShipmentRepository>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _handler = new AdvanceShipmentStatusCommandHandler(
            _mockUnitOfWork.Object,
            _mockShipmentRepository.Object,
            Mock.Of<ILogger<AdvanceShipmentStatusCommandHandler>>()
        );
    }

    /// <summary>
    /// Verifies the shipment status advances when the shipment exists and is not pending.
    /// </summary>
    [Fact]
    public async Task HandleAdvanceShipmentStatusCommand_WithExistentPreparingShipment_AdvancesStatusToTheNextState()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();
        var shipment = ShipmentUtils.CreateShipment(
            id: ShipmentId.Create(1),
            initialShipmentStatus: ShipmentStatus.Preparing
        );

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShipmentId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(shipment);

        await _handler.Handle(command, default);

        shipment.ShipmentStatus.Should().Be(ShipmentStatus.Shipped);

        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    /// <summary>
    /// Verifies an error is thrown when trying to advance a pending shipment manually.
    /// </summary>
    [Fact]
    public async Task HandleAdvanceShipmentStatusCommand_WithExistentPendingShipment_ThrowsError()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();
        var shipment = ShipmentUtils.CreateShipment(id: ShipmentId.Create(1));

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShipmentId>(),
                It.IsAny<CancellationToken>()
            ))
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
    public async Task HandleAdvanceShipmentStatusCommand_WhenShipmentDoesNotExist_ThrowsError()
    {
        var command = AdvanceShipmentStatusCommandUtils.CreateCommand();

        _mockShipmentRepository
            .Setup(r => r.FindByIdAsync(
                It.IsAny<ShipmentId>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync((Shipment?)null);

        await FluentActions
            .Invoking(() => _handler.Handle(command, default))
            .Should()
            .ThrowAsync<ShipmentNotFoundException>();
    }
}
