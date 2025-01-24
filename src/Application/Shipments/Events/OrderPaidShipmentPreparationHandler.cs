using Application.Common.Persistence;
using Domain.OrderAggregate.Events;
using Domain.ShipmentAggregate;

using MediatR;

namespace Application.Shipments.Events;

/// <summary>
/// Handles the <see cref="OrderPaid"/> event by
/// creating a shipment.
/// </summary>
public class OrderPaidShipmentPreparationHandler : INotificationHandler<OrderPaid>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderPaidShipmentPreparationHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public OrderPaidShipmentPreparationHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(OrderPaid notification, CancellationToken cancellationToken)
    {
        var shipment = Shipment.Create(
            notification.Order.Id,
            "ECommerceManagementShipmentService"
        );

        await _unitOfWork.ShipmentRepository.AddAsync(shipment);

        await _unitOfWork.SaveChangesAsync();
    }
}
