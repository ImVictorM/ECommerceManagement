using Application.Common.Persistence;

using Domain.OrderAggregate.Events;

using MediatR;

namespace Application.Shipments.Events;

/// <summary>
/// Handles the <see cref="OrderCanceled"/> event by
/// canceling the related shipment.
/// </summary>
public sealed class OrderCanceledCancelShipmentHandler : INotificationHandler<OrderCanceled>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShipmentRepository _shipmentRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCanceledCancelShipmentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="shipmentRepository">The shipment repository.</param>
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
