using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;
using Application.Shipments.Errors;

using Domain.OrderAggregate.Events;
using Domain.ShipmentAggregate.Enumerations;

using MediatR;

namespace Application.Shipments.Events;

internal sealed class OrderPaidPrepareShipmentHandler
    : INotificationHandler<OrderPaid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentRepository _shipmentRepository;

    public OrderPaidPrepareShipmentHandler(
        IUnitOfWork unitOfWork,
        IShipmentRepository shipmentRepository
    )
    {
        _unitOfWork = unitOfWork;
        _shipmentRepository = shipmentRepository;
    }

    public async Task Handle(
        OrderPaid notification,
        CancellationToken cancellationToken
    )
    {
        var shipment = await _shipmentRepository.GetShipmentByOrderId(
            notification.Order.Id,
            cancellationToken
        );

        if (shipment == null)
        {
            return;
        }

        if (shipment.ShipmentStatus != ShipmentStatus.Pending)
        {
            throw new PrepareShipmentNotPendingException(shipment.ShipmentStatus);
        }

        shipment.AdvanceShipmentStatus();

        await _unitOfWork.SaveChangesAsync();
    }
}
