using Application.Common.Errors;
using Application.Common.Persistence;

using Domain.OrderAggregate.Events;
using Domain.ShipmentAggregate.Enumerations;

using MediatR;

namespace Application.Shipments.Events;

/// <summary>
/// Handles the <see cref="OrderPaid"/> event by
/// Advancing the shipment status to <see cref="Domain.ShipmentAggregate.Enumerations.ShipmentStatus.Preparing"/>.
/// </summary>
public sealed class OrderPaidPrepareShipmentHandler : INotificationHandler<OrderPaid>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderPaidPrepareShipmentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderPaidPrepareShipmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderPaid notification, CancellationToken cancellationToken)
    {
        var shipment = await _unitOfWork.ShipmentRepository.FindOneOrDefaultAsync(s => s.OrderId == notification.Order.Id);

        if (shipment == null)
        {
            return;
        }

        if (shipment.ShipmentStatus != ShipmentStatus.Pending)
        {
            throw new OperationProcessFailedException($"Shipment status was expected to be 'Pending' but was '{shipment.ShipmentStatus.Name}' instead");
        }

        shipment.AdvanceShipmentStatus();

        await _unitOfWork.SaveChangesAsync();
    }
}
