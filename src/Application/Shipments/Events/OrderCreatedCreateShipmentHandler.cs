using Domain.OrderAggregate.Events;
using Domain.ShipmentAggregate;

using Application.Common.Persistence.Repositories;
using Application.Common.Persistence;

using MediatR;

namespace Application.Shipments.Events;

internal sealed class OrderCreatedCreateShipmentHandler
    : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICarrierRepository _carrierRepository;
    private readonly IShipmentRepository _shipmentRepository;

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

    public async Task Handle(
        OrderCreated notification,
        CancellationToken cancellationToken
    )
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
