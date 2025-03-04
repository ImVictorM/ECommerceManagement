using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Sales;

/// <summary>
/// Defines a contract to provide seed data for sales in the database.
/// </summary>
public interface ISaleSeed : IDataSeed<SaleSeedType, Sale, SaleId>
{
}
