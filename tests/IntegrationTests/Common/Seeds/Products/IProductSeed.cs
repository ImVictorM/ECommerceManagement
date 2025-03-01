using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Products;

/// <summary>
/// Defines a contract to provide seed data for products in the database.
/// </summary>
public interface IProductSeed : IDataSeed<ProductSeedType, Product, ProductId>
{
}
