using Application.Orders.Commands.Common.DTOs;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

namespace IntegrationTests.TestUtils.Seeds;

public enum SeedAvailableOrders
{
    CUSTOMER_ORDER_PENDING
}

public static class OrderSeed
{
    private static Dictionary<SeedAvailableOrders, Order> _orders = [];

    /// <summary>
    /// Initializes the seed orders asynchronously.
    /// </summary>
    public static async Task InitializeAsync()
    {
        _orders = new Dictionary<SeedAvailableOrders, Order>
        {
            [SeedAvailableOrders.CUSTOMER_ORDER_PENDING] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-1),
                ownerId: UserSeed.GetSeedUser(SeedAvailableUsers.Customer).Id,
                orderProducts: [
                    new OrderProductInput(ProductSeed.GetSeedProduct(SeedAvailableProducts.TSHIRT).Id.ToString(), 1)
                ],
                installments: 1
            )
        };
    }

    /// <summary>
    /// Retrieves a seed order by type.
    /// </summary>
    public static Order GetSeedOrder(SeedAvailableOrders orderType)
    {
        if (_orders.TryGetValue(orderType, out var order))
        {
            return order;
        }
        throw new InvalidOperationException($"Order of type {orderType} has not been initialized.");
    }

    /// <summary>
    /// Lists all seed orders.
    /// </summary>
    public static IEnumerable<Order> ListOrders()
    {
        if (_orders.Count == 0)
        {
            throw new InvalidOperationException($"To list the orders, you must initialize it first");
        }

        return _orders.Values;
    }
}
