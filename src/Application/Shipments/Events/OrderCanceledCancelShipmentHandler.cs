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

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCanceledCancelShipmentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCanceledCancelShipmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCanceled notification, CancellationToken cancellationToken)
    {
        var shipmentToBeCanceled = await _unitOfWork.ShipmentRepository.FindOneOrDefaultAsync(s => s.OrderId == notification.Order.Id);

        shipmentToBeCanceled?.Cancel();

        await _unitOfWork.SaveChangesAsync();
    }
}
