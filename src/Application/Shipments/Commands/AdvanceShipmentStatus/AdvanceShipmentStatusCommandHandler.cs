using Application.Common.Persistence;
using Application.Shipments.Errors;

using Domain.ShipmentAggregate.ValueObjects;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Shipments.Commands.AdvanceShipmentStatus;

/// <summary>
/// Handles the <see cref="AdvanceShipmentStatusCommand"/> command.
/// </summary>
public sealed partial class AdvanceShipmentStatusCommandHandler : IRequestHandler<AdvanceShipmentStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="AdvanceShipmentStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public AdvanceShipmentStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AdvanceShipmentStatusCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Unit> Handle(AdvanceShipmentStatusCommand request, CancellationToken cancellationToken)
    {
        LogInitiatingAdvanceShipmentStatus(request.ShipmentId);

        var shipmentId = ShipmentId.Create(request.ShipmentId);

        var shipment = await _unitOfWork.ShipmentRepository.FindByIdAsync(shipmentId);

        if (shipment == null)
        {
            LogShipmentNotFound();

            throw new ShipmentNotFoundException(
                "It was not possible to advance the shipment status because the shipment was not found"
            );
        }

        LogCurrentShipmentStatus(shipment.ShipmentStatus.Name);

        shipment.AdvanceShipmentStatus();

        LogShipmentStatusAdvancedTo(shipment.ShipmentStatus.Name);

        await _unitOfWork.SaveChangesAsync();

        LogShipmentStatusAdvancedSuccessfully();

        return Unit.Value;
    }
}
