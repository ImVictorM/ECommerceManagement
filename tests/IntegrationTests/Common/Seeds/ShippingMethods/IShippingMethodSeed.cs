using Domain.ShippingMethodAggregate;
using Domain.ShippingMethodAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.ShippingMethods;

/// <summary>
/// Defines a contract to provide seed data for shipping methods in the database.
/// </summary>
public interface IShippingMethodSeed
    : IDataSeed<ShippingMethodSeedType, ShippingMethod, ShippingMethodId>
{
}
