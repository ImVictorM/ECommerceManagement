using Domain.ProductFeedbackAggregate.ValueObjects;
using DomainProductFeedback = Domain.ProductFeedbackAggregate.ProductFeedback;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.ProductFeedback;

/// <summary>
/// Defines a contract to provide seed data for product feedback in the database.
/// </summary>
public interface IProductFeedbackSeed
    : IDataSeed<ProductFeedbackSeedType, DomainProductFeedback, ProductFeedbackId>
{
}
