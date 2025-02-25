using Application.Common.Persistence;
using Application.Common.Persistence.Repositories;

using Domain.OrderAggregate.Events;

using MediatR;

namespace Application.Shipments.Events;

internal sealed class OrderCanceledCancelShipmentHandler : INotificationHandler<OrderCanceled>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentRepository _shipmentRepository;

    public OrderCanceledCancelShipmentHandler(IUnitOfWork unitOfWork, IShipmentRepository shipmentRepository)
    {
        _unitOfWork = unitOfWork;
        _shipmentRepository = shipmentRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
    {
        var shipmentToBeCanceled = await _shipmentRepository.GetShipmentByOrderId(
            notification.Order.Id,
            cancellationToken
        );

        if (shipmentToBeCanceled == null)
        {
            return;
        }

        shipmentToBeCanceled.Cancel();

        _shipmentRepository.Update(shipmentToBeCanceled);

        await _unitOfWork.SaveChangesAsync();
    }
}
