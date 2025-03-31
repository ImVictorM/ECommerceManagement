using Application.ProductReviews.DTOs.Results;
using Application.ProductReviews.Queries.Projections;

using Domain.ProductReviewAggregate.ValueObjects;

using FluentAssertions;

namespace Application.UnitTests.ProductReviews.TestUtils.Extensions;

/// <summary>
/// Utilities for the <see cref="ProductReviewDetailedResult"/> class.
/// </summary>
public static class ProductReviewDetailedResultExtensions
{
    /// <summary>
    /// Ensures an <see cref="ProductReviewDetailedResult"/> matches the results
    /// from an <see cref="ProductReviewDetailedProjection"/> projection.
    /// </summary>
    /// <param name="result">The current result.</param>
    /// <param name="projection">The projection to be compared.</param>
    public static void EnsureCorrespondsTo(
        this ProductReviewDetailedResult result,
        ProductReviewDetailedProjection projection
    )
    {
        result.Id.Should().Be(projection.Id.ToString());
        result.Title.Should().Be(projection.Title);
        result.Content.Should().Be(projection.Content);
        result.StarRating.Should().Be(projection.StarRating.Value);
        result.CreatedAt.Should().Be(projection.CreatedAt);
        result.UpdatedAt.Should().Be(projection.UpdatedAt);
        result.User.Id.Should().Be(projection.User.Id.ToString());
        result.User.Name.Should().Be(projection.User.Name);
    }

    /// <summary>
    /// Ensures a collection of <see cref="ProductReviewDetailedResult"/> matches
    /// the results from a collection of <see cref="ProductReviewDetailedProjection"/>.
    /// </summary>
    /// <param name="results">The current results.</param>
    /// <param name="projections">The projections to be compared.</param>
    public static void EnsureCorrespondsTo(
        this IEnumerable<ProductReviewDetailedResult> results,
        IEnumerable<ProductReviewDetailedProjection> projections
    )
    {
        foreach (var result in results)
        {
            var projection = projections
                .FirstOrDefault(p => p.Id == ProductReviewId.Create(result.Id));

            projection.Should().NotBeNull();
            result.EnsureCorrespondsTo(projection!);
        }
    }
}
