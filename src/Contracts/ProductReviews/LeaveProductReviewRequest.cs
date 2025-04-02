namespace Contracts.ProductReviews;

/// <summary>
/// Represents a request to leave a product review.
/// </summary>
/// <param name="Title">The review title.</param>
/// <param name="Content">The review content.</param>
/// <param name="StarRating">The review star rating.</param>
public record LeaveProductReviewRequest(
    string Title,
    string Content,
    int StarRating
);
