using Domain.CategoryAggregate;
using Domain.CategoryAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.Categories;

/// <summary>
/// Defines a contract to provide seed data for categories in the database.
/// </summary>
public interface ICategorySeed : IDataSeed<CategorySeedType, Category, CategoryId>
{
}
