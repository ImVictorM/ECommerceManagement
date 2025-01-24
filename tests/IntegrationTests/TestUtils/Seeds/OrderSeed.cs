using Application.Orders.Commands.Common.DTOs;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate.ValueObjects;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// The available order types in the database seed.
/// </summary>
public enum SeedAvailableOrders
{
    /// <summary>
    /// Represents a pending order.
    /// </summary>
    CUSTOMER_ORDER_PENDING,
    /// <summary>
    /// Represents a canceled order.
    /// </summary>
    CUSTOMER_ORDER_CANCELED,
    /// <summary>
    /// Represents a paid order.
    /// </summary>
    CUSTOMER_ORDER_PAID,
}

/// <summary>
/// Contain order seed data.
/// </summary>
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
            ),
            [SeedAvailableOrders.CUSTOMER_ORDER_CANCELED] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-2),
                ownerId: UserSeed.GetSeedUser(SeedAvailableUsers.Customer).Id,
                orderProducts: [
                     new OrderProductInput(ProductSeed.GetSeedProduct(SeedAvailableProducts.PENCIL).Id.ToString(), 1)
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Canceled
            ),
            [SeedAvailableOrders.CUSTOMER_ORDER_PAID] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-3),
                ownerId: UserSeed.GetSeedUser(SeedAvailableUsers.Customer).Id,
                orderProducts: [
                     new OrderProductInput(ProductSeed.GetSeedProduct(SeedAvailableProducts.CHAIN_BRACELET).Id.ToString(), 1)
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Paid
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
        throw new InvalidOperationException("Order of type {orderType} has not been initialized.");
    }

    /// <summary>
    /// Lists all seed orders.
    /// </summary>
    public static IReadOnlyList<Order> ListOrders()
    {
        if (_orders.Count == 0)
        {
            throw new InvalidOperationException("To list the orders, you must initialize it first");
        }

        return [.. _orders.Values];
    }

    /// <summary>
    /// Retrieves all the seed orders of an user.
    /// </summary>
    /// <param name="ownerId">The user id.</param>
    /// <returns>A list of the user orders.</returns>
    public static IReadOnlyList<Order> GetUserOrders(UserId ownerId)
    {
        return _orders.Values.Where(o => o.OwnerId == ownerId).ToList();
    }
}
