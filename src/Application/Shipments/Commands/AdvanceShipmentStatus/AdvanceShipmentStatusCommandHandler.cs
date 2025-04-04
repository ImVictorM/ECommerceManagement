using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;
using Application.Shipments.Errors;

using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;

using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Shipments.Commands.AdvanceShipmentStatus;

internal sealed partial class AdvanceShipmentStatusCommandHandler
    : IRequestHandler<AdvanceShipmentStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentRepository _shipmentRepository;

    public AdvanceShipmentStatusCommandHandler(
        IUnitOfWork unitOfWork,
        IShipmentRepository shipmentRepository,
        ILogger<AdvanceShipmentStatusCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _shipmentRepository = shipmentRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        AdvanceShipmentStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        LogInitiatingAdvanceShipmentStatus(request.ShipmentId);

        var shipmentId = ShipmentId.Create(request.ShipmentId);

        var shipment = await _shipmentRepository.FindByIdAsync(
            shipmentId,
            cancellationToken
        );

        if (shipment == null)
        {
            LogShipmentNotFound();

            throw new ShipmentNotFoundException(
                "It was not possible to advance the shipment status because" +
                " the shipment was not found"
            );
        }

        if (shipment.ShipmentStatus == ShipmentStatus.Pending)
        {
            throw new AdvancePendingShipmentStatusException();
        }

        LogCurrentShipmentStatus(shipment.ShipmentStatus.Name);

        shipment.AdvanceShipmentStatus();

        LogShipmentStatusAdvancedTo(shipment.ShipmentStatus.Name);

        await _unitOfWork.SaveChangesAsync();

        LogShipmentStatusAdvancedSuccessfully();

        return Unit.Value;
    }
}
