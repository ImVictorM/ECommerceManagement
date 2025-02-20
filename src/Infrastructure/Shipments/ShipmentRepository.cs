using Application.Common.Persistence;

using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Shipments;

/// <summary>
/// Defines the implementation for shipment persistence operations.
/// </summary>
public sealed class ShipmentRepository : BaseRepository<Shipment, ShipmentId>, IShipmentRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ShipmentRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public Task<Shipment?> GetShipmentByOrderId(OrderId orderId, CancellationToken cancellationToken = default)
    {
        return FirstOrDefaultAsync(shipment => shipment.OrderId == orderId, cancellationToken);
    }
}
