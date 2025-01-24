using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;

using SharedKernel.Models;

namespace Application.Common.Extensions.Orders;

/// <summary>
/// Extensions for the <see cref="Order"/> class.
/// </summary>
public static class OrderExtensions
{
    /// <summary>
    /// Retrieves the order status description.
    /// </summary>
    /// <param name="order">The current order.</param>
    /// <returns>The description of the order status.</returns>
    public static string GetStatusDescription(this Order order)
    {
        var status = BaseEnumeration.FromValue<OrderStatus>(order.OrderStatusId);

        return status.Name;
    }
}
