using Domain.ProductAggregate.ValueObjects;
using Domain.ProductReviewAggregate.ValueObjects;

namespace Application.ProductReviews.Queries.Projections;

/// <summary>
/// Represents a detailed product review projection.
/// </summary>
/// <param name="Id">The review identifier.</param>
/// <param name="ProductId">The product identifier.</param>
/// <param name="Title">The review title.</param>
/// <param name="Content">The review content.</param>
/// <param name="StarRating">The review star rating.</param>
/// <param name="CreatedAt">The date the review was posted.</param>
/// <param name="UpdatedAt">The last time the review was updated.</param>
/// <param name="User">The user who posted the review.</param>
public record ProductReviewDetailedProjection(
    ProductReviewId Id,
    ProductId ProductId,
    string Title,
    string Content,
    StarRating StarRating,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    ProductReviewUserProjection User
);
