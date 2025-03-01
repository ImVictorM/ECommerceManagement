using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Orders;

/// <summary>
/// Defines a contract to provide seed data for orders in the database.
/// </summary>
public interface IOrderSeed : IAsyncDataSeed<OrderSeedType, Order, OrderId>
{
}
