using Domain.ProductReviewAggregate;
using Domain.ProductReviewAggregate.ValueObjects;

using IntegrationTests.Common.Seeds.Abstracts;

namespace IntegrationTests.Common.Seeds.ProductReviews;

/// <summary>
/// Defines a contract to provide seed data for product reviews in the database.
/// </summary>
public interface IProductReviewSeed
    : IDataSeed<ProductReviewSeedType, ProductReview, ProductReviewId>
{
}
