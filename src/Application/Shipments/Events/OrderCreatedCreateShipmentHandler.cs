using Domain.OrderAggregate.Events;
using Domain.ShipmentAggregate;

using Application.Common.Persistence;

using MediatR;

namespace Application.Shipments.Events;

/// <summary>
/// Handles the <see cref="OrderPaid"/> event by
/// creating a shipment.
/// </summary>
public class OrderCreatedCreateShipmentHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedCreateShipmentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderCreatedCreateShipmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var shipmentCarrier = await _unitOfWork.CarrierRepository.FirstAsync();

        var shipment = Shipment.Create(
            notification.Order.Id,
            shipmentCarrier.Id,
            notification.ShippingMethodId,
            notification.DeliveryAddress
        );

        await _unitOfWork.ShipmentRepository.AddAsync(shipment);

        await _unitOfWork.SaveChangesAsync();
    }
}
