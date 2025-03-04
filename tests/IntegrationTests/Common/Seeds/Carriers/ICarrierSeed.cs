using Domain.CarrierAggregate;
using Domain.CarrierAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Carriers;

/// <summary>
/// Defines a contract to provide seed data for carriers in the database.
/// </summary>
public interface ICarrierSeed : IDataSeed<CarrierSeedType, Carrier, CarrierId>
{
}
