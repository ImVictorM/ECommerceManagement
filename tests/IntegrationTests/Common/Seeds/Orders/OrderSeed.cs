using Application.Orders.DTOs;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ShippingMethodAggregate;
using Domain.UnitTests.TestUtils;
using Domain.UserAggregate;
using Domain.UserAggregate.ValueObjects;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Seeds.Orders;

/// <summary>
/// Provides seed data for orders in the database.
/// </summary>
public sealed class OrderSeed : AsyncDataSeed<OrderSeedType, Order>
{
    private readonly IDataSeed<UserSeedType, User> _userSeed;
    private readonly IDataSeed<ProductSeedType, Product> _productSeed;
    private readonly IDataSeed<ShippingMethodSeedType, ShippingMethod> _shippingMethodSeed;

    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderSeed"/> class.
    /// </summary>
    /// <param name="userSeed">The user seed.</param>
    /// <param name="productSeed">The product seed.</param>
    /// <param name="shippingMethodSeed">The shipping method seed.</param>
    public OrderSeed(
        IDataSeed<UserSeedType, User> userSeed,
        IDataSeed<ProductSeedType, Product> productSeed,
        IDataSeed<ShippingMethodSeedType, ShippingMethod> shippingMethodSeed
    ) : base()
    {
        _userSeed = userSeed;
        _productSeed = productSeed;
        _shippingMethodSeed = shippingMethodSeed;

        Order = productSeed.Order + userSeed.Order + shippingMethodSeed.Order + 20;
    }

    /// <inheritdoc/>
    public override async Task InitializeAsync()
    {
        var orders = new Dictionary<OrderSeedType, Order>
        {
            [OrderSeedType.CUSTOMER_ORDER_PENDING] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-1),
                ownerId: _userSeed.GetByType(UserSeedType.CUSTOMER).Id,
                orderProducts:
                [
                    new OrderProductInput(_productSeed.GetByType(ProductSeedType.TSHIRT).Id.ToString(), 1)
                ],
                installments: 1,
                shippingMethodId: _shippingMethodSeed.GetByType(ShippingMethodSeedType.FREE).Id
            ),
            [OrderSeedType.CUSTOMER_ORDER_CANCELED] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-2),
                ownerId: _userSeed.GetByType(UserSeedType.CUSTOMER).Id,
                orderProducts:
                [
                    new OrderProductInput(_productSeed.GetByType(ProductSeedType.PENCIL).Id.ToString(), 1)
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Canceled,
                shippingMethodId: _shippingMethodSeed.GetByType(ShippingMethodSeedType.FREE).Id
            ),
            [OrderSeedType.CUSTOMER_ORDER_PAID] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-3),
                ownerId: _userSeed.GetByType(UserSeedType.CUSTOMER).Id,
                orderProducts:
                [
                    new OrderProductInput(_productSeed.GetByType(ProductSeedType.CHAIN_BRACELET).Id.ToString(), 1)
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Paid,
                shippingMethodId: _shippingMethodSeed.GetByType(ShippingMethodSeedType.EXPRESS).Id
            )
        };

        foreach (var (key, value) in orders)
        {
            Data[key] = value;
        }
    }

    /// <summary>
    /// Retrieves all the seed orders associated with a specific user.
    /// </summary>
    /// <param name="ownerId">The user id.</param>
    /// <returns>A read-only list of orders belonging to the specified user.</returns>
    public IReadOnlyList<Order> GetUserOrders(UserId ownerId)
    {
        return Data.Values.Where(o => o.OwnerId == ownerId).ToList();
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await InitializeAsync();

        await context.Orders.AddRangeAsync(ListAll());
    }
}
