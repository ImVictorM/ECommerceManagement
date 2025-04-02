using Domain.UserAggregate.ValueObjects;

namespace Application.ProductReviews.Queries.Projections;

/// <summary>
/// Represents a review user projection.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Name">The user name.</param>
public record ProductReviewUserProjection(
    UserId Id,
    string Name
);
