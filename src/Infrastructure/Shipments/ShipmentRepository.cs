using Application.Common.Persistence.Repositories;

using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShipmentAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

namespace Infrastructure.Shipments;

internal sealed class ShipmentRepository : BaseRepository<Shipment, ShipmentId>, IShipmentRepository
{
    public ShipmentRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public Task<Shipment?> GetShipmentByOrderId(OrderId orderId, CancellationToken cancellationToken = default)
    {
        return FirstOrDefaultAsync(shipment => shipment.OrderId == orderId, cancellationToken);
    }
}
