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
    private readonly ICarrierRepository _carrierRepository;
    private readonly IShipmentRepository _shipmentRepository;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderCreatedCreateShipmentHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="carrierRepository">The carrier repository.</param>
    /// <param name="shipmentRepository">The shipment repository.</param>
    public OrderCreatedCreateShipmentHandler(
        IUnitOfWork unitOfWork,
        ICarrierRepository carrierRepository,
        IShipmentRepository shipmentRepository
    )
    {
        _unitOfWork = unitOfWork;
        _carrierRepository = carrierRepository;
        _shipmentRepository = shipmentRepository;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var shipmentCarrier = await _carrierRepository.FirstAsync(cancellationToken);

        var shipment = Shipment.Create(
            notification.Order.Id,
            shipmentCarrier.Id,
            notification.ShippingMethodId,
            notification.DeliveryAddress
        );

        await _shipmentRepository.AddAsync(shipment);

        await _unitOfWork.SaveChangesAsync();
    }
}
