using Application.Orders.DTOs;

using Domain.OrderAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Domain.ShipmentAggregate;
using Domain.ShippingMethodAggregate;

namespace Application.UnitTests.Orders.TestUtils;

/// <summary>
/// Utilities for the <see cref="OrderDetailedQueryResult"/> class.
/// </summary>
public static class OrderDetailedQueryResultUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderDetailedQueryResult"/> class.
    /// </summary>
    /// <param name="order">The order.</param>
    /// <param name="shipment">The shipment.</param>
    /// <param name="shippingMethod">The shipping method.</param>
    /// <param name="paymentId">The payment id.</param>
    /// <returns>A new instance of the <see cref="OrderDetailedQueryResult"/> class.</returns>
    public static OrderDetailedQueryResult CreateResult(
        Order order,
        Shipment shipment,
        ShippingMethod shippingMethod,
        PaymentId paymentId
    )
    {
        return new OrderDetailedQueryResult(
            order,
            new OrderShipmentResult(
                shipment.Id,
                shipment.ShipmentStatus.Name,
                shipment.DeliveryAddress,
                new OrderShippingMethodResult(
                    shippingMethod.Name,
                    shippingMethod.Price,
                    shippingMethod.EstimatedDeliveryDays
                )
            ),
            paymentId
        );
    }
}
