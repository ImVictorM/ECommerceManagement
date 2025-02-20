using Application.Common.Persistence;
using Application.Orders.DTOs;

using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Orders;

/// <summary>
/// Defines the implementation for order persistence operations.
/// </summary>
public sealed class OrderRepository : BaseRepository<Order, OrderId>, IOrderRepository
{
    /// <summary>
    /// Initiates a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public OrderRepository(ECommerceDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedQueryResult?> GetOrderDetailedAsync(
        OrderId orderId,
        UserId ownerId,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Orders
            .Where(order => order.Id == orderId && order.OwnerId == ownerId)
            .Select(order => new OrderDetailedQueryResult(
                order,
                GetOrderShipmentResult(orderId),
                GetOrderPaymentId(orderId)
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<OrderDetailedQueryResult?> GetOrderDetailedAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default
    )
    {
        return await Context.Orders
            .Where(order => order.Id == orderId)
            .Select(order => new OrderDetailedQueryResult(
                order,
                GetOrderShipmentResult(orderId),
                GetOrderPaymentId(orderId)
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private OrderShipmentResult GetOrderShipmentResult(OrderId orderId)
    {
        return Context.Shipments
            .Where(shipment => shipment.OrderId == orderId)
            .Select(shipment => new OrderShipmentResult(
                shipment.Id,
                shipment.ShipmentStatus.Name,
                shipment.DeliveryAddress,
                GetOrderShipmentShippingMethodResult(shipment.ShippingMethodId)
            ))
            .First();
    }

    private OrderShippingMethodResult GetOrderShipmentShippingMethodResult(ShippingMethodId shippingMethodId)
    {
        return Context.ShippingMethods
            .Where(method => method.Id == shippingMethodId)
            .Select(method => new OrderShippingMethodResult(
                method.Name,
                method.Price,
                method.EstimatedDeliveryDays
            ))
            .First();
    }

    private PaymentId GetOrderPaymentId(OrderId orderId)
    {
        return Context.Payments
            .Where(payment => payment.OrderId == orderId)
            .Select(payment => payment.Id)
            .First();
    }
}
