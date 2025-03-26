using Application.Common.Persistence.Repositories;
using Application.Orders.Queries.Projections;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using SharedKernel.Models;

using Microsoft.EntityFrameworkCore;
using Application.Orders.DTOs.Filters;

namespace Infrastructure.Orders;

internal sealed class OrderRepository
    : BaseRepository<Order, OrderId>, IOrderRepository
{
    public OrderRepository(IECommerceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OrderDetailedProjection?> GetCustomerOrderDetailedAsync(
        OrderId orderId,
        UserId ownerId,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Orders
            .Where(order => order.Id == orderId && order.OwnerId == ownerId)
            .Select(order => new OrderDetailedProjection(
                order.Id,
                order.OwnerId,
                order.Description,
                order.OrderStatus,
                order.Total,
                Context.Shipments
                    .Where(shipment => shipment.OrderId == orderId)
                    .Select(shipment => new OrderShipmentProjection(
                        shipment.Id,
                        shipment.ShipmentStatus,
                        shipment.DeliveryAddress,
                        Context.ShippingMethods
                            .Where(method => method.Id == shipment.ShippingMethodId)
                            .Select(method => new OrderShippingMethodProjection(
                                method.Name,
                                method.Price,
                                method.EstimatedDeliveryDays
                            ))
                            .First()
                    ))
                    .First(),
                Context.Payments
                    .Where(payment => payment.OrderId == orderId)
                    .Select(payment => payment.Id)
                    .First()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OrderDetailedProjection?> GetOrderDetailedAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Orders
            .Where(order => order.Id == orderId)
            .Select(order => new OrderDetailedProjection(
                order.Id,
                order.OwnerId,
                order.Description,
                order.OrderStatus,
                order.Total,
                Context.Shipments
                    .Where(shipment => shipment.OrderId == orderId)
                    .Select(shipment => new OrderShipmentProjection(
                        shipment.Id,
                        shipment.ShipmentStatus,
                        shipment.DeliveryAddress,
                        Context.ShippingMethods
                            .Where(method => method.Id == shipment.ShippingMethodId)
                            .Select(method => new OrderShippingMethodProjection(
                                method.Name,
                                method.Price,
                                method.EstimatedDeliveryDays
                            ))
                            .First()
                    ))
                    .First(),
                Context.Payments
                    .Where(payment => payment.OrderId == orderId)
                    .Select(payment => payment.Id)
                    .First()
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderProjection>> GetCustomerOrdersAsync(
        UserId ownerId,
        OrderFilters? filters = default,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        query = query.Where(order => order.OwnerId == ownerId);

        if (!string.IsNullOrWhiteSpace(filters?.Status))
        {
            var status = BaseEnumeration.FromDisplayName<OrderStatus>(filters.Status);

            query = query.Where(order => order.OrderStatus == status);
        }

        return await query
            .AsNoTracking()
            .Select(order => new OrderProjection(
                order.Id,
                order.OwnerId,
                order.Description,
                order.OrderStatus,
                order.Total
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OrderProjection>> GetOrdersAsync(
        OrderFilters? filters = default,
        CancellationToken cancellationToken = default
    )
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters?.Status))
        {
            var status = BaseEnumeration.FromDisplayName<OrderStatus>(filters.Status);

            query = query.Where(order => order.OrderStatus == status);
        }

        return await query
            .AsNoTracking()
            .Select(order => new OrderProjection(
                order.Id,
                order.OwnerId,
                order.Description,
                order.OrderStatus,
                order.Total
            ))
            .ToListAsync(cancellationToken);
    }
}
