using Application.Orders.Queries.Projections;

using Domain.OrderAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShippingMethodAggregate;

namespace Application.UnitTests.Orders.TestUtils.Projections;

/// <summary>
/// Utilities for the <see cref="OrderDetailedProjection"/> class.
/// </summary>
public static class OrderDetailedProjectionUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderDetailedProjection"/> class.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="shipment">The shipment.</param>
    /// <param name="shippingMethod">The shipping method.</param>
    /// <param name="paymentId">The payment identifier.</param>
    /// <returns>
    /// A new instance of the <see cref="OrderDetailedProjection"/> class.
    /// </returns>
    public static OrderDetailedProjection CreateProjection(
        Order order,
        Shipment shipment,
        ShippingMethod shippingMethod,
        PaymentId paymentId
    )
    {
        return new OrderDetailedProjection(
            order.Id,
            order.OwnerId,
            order.Description,
            order.OrderStatus,
            order.Total,
            order.Products,
            new OrderShipmentProjection(
                shipment.Id,
                shipment.ShipmentStatus,
                shipment.DeliveryAddress,
                new OrderShippingMethodProjection(
                    shippingMethod.Name,
                    shippingMethod.Price,
                    shippingMethod.EstimatedDeliveryDays
                )
            ),
            paymentId
        );
    }
}
