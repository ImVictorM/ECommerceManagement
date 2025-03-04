using Application.Orders.DTOs;

using Domain.OrderAggregate;
using Domain.OrderAggregate.Enumerations;
using Domain.OrderAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.ShippingMethods;
using IntegrationTests.Common.Seeds.Users;

namespace IntegrationTests.Common.Seeds.Orders;

/// <summary>
/// Provides seed data for orders in the database.
/// </summary>
public sealed class OrderSeed
    : AsyncDataSeed<OrderSeedType, Order, OrderId>, IOrderSeed
{
    private readonly IUserSeed _userSeed;
    private readonly IProductSeed _productSeed;
    private readonly IShippingMethodSeed _shippingMethodSeed;

    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderSeed"/> class.
    /// </summary>
    /// <param name="userSeed">The user seed.</param>
    /// <param name="productSeed">The product seed.</param>
    /// <param name="shippingMethodSeed">The shipping method seed.</param>
    public OrderSeed(
        IUserSeed userSeed,
        IProductSeed productSeed,
        IShippingMethodSeed shippingMethodSeed
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
                ownerId: _userSeed.GetEntityId(UserSeedType.CUSTOMER),
                orderProducts:
                [
                    new OrderProductInput(
                        _productSeed
                            .GetEntityId(ProductSeedType.TSHIRT)
                            .ToString(),
                        1
                    )
                ],
                installments: 1,
                shippingMethodId: _shippingMethodSeed.GetEntityId(
                    ShippingMethodSeedType.FREE
                )
            ),
            [OrderSeedType.CUSTOMER_ORDER_CANCELED] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-2),
                ownerId: _userSeed.GetEntityId(UserSeedType.CUSTOMER),
                orderProducts:
                [
                    new OrderProductInput(
                        _productSeed
                            .GetEntityId(ProductSeedType.PENCIL)
                            .ToString(),
                        1
                    )
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Canceled,
                shippingMethodId: _shippingMethodSeed.GetEntityId(
                    ShippingMethodSeedType.FREE
                )
            ),
            [OrderSeedType.CUSTOMER_ORDER_PAID] = await OrderUtils.CreateOrderAsync(
                id: OrderId.Create(-3),
                ownerId: _userSeed.GetEntityId(UserSeedType.CUSTOMER),
                orderProducts:
                [
                    new OrderProductInput(
                        _productSeed
                            .GetEntityId(ProductSeedType.CHAIN_BRACELET)
                            .ToString(),
                        1
                    )
                ],
                installments: 1,
                initialOrderStatus: OrderStatus.Paid,
                shippingMethodId: _shippingMethodSeed.GetEntityId(
                    ShippingMethodSeedType.EXPRESS
                )
            )
        };

        foreach (var (key, value) in orders)
        {
            Data[key] = value;
        }
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await InitializeAsync();

        await context.Orders.AddRangeAsync(ListAll());
    }
}
