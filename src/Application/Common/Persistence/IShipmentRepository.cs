using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;

namespace Application.Common.Persistence;

/// <summary>
/// Defines the contract for shipment persistence operations.
/// </summary>
public interface IShipmentRepository : IBaseRepository<Shipment, ShipmentId>
{
    /// <summary>
    /// Retrieves a shipment related to the specified order identifier.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The shipment related to the specified order.</returns>
    Task<Shipment?> GetShipmentByOrderId(
        OrderId orderId,
        CancellationToken cancellationToken = default
    );
}
